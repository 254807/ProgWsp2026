using System;
using System.Collections.Generic;
using ConcurrentProgramming.Data;
using ConcurrentProgramming.Logic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ConcurrentProgramming.Model;

namespace ConcurrentProgramming.ViewModel;

public sealed class MainViewModel : INotifyPropertyChanged
{
    public IBallLogic BallLogic { get; }

    public ObservableCollection<BallModel> Balls { get; } = [];
    
    public ICommand AddBallsCommand { get; }

    public int BallsToAdd
    {
        get;
        set => SetField(ref field, value);
    } = 0;

    public MainViewModel()
    {
        BallLogic = new BallLogic();
        ((INotifyCollectionChanged)BallLogic.Balls).CollectionChanged += BallsOnCollectionChanged;  // TOTAL .NET INSANITY

        AddBallsCommand = new DelegateCommand(ExecuteAddBallsCommand, _ => BallLogic.Balls.Count == 0);
    }

    private void ExecuteAddBallsCommand(object? parameter)
    {
        AddBalls(BallsToAdd);
        BallLogic.RunMainLoop();
    }

    private void BallsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Add) return;
        
        foreach (var newItem in e.NewItems?.OfType<IBall>() ?? [])
        {
            Balls.Add(new BallModel(newItem));
        }
    }

    public void AddBalls(int count)
    {
        BallLogic.AddBalls(count);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
