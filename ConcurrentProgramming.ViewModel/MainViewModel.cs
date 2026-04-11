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

/// <summary>
/// The main view model.
/// </summary>
public sealed class MainViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the ball logic.
    /// </summary>
    public IBallLogic BallLogic { get; }

    /// <summary>
    /// Gets the balls.
    /// </summary>
    public ObservableCollection<BallModel> Balls { get; } = [];

    public ICommand AddBallsCommand { get; }

    public int BallsToAdd
    {
        get;
        set => SetField(ref field, value);
    } = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
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

    /// <summary>
    /// Balls the on collection changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void BallsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Add) return;
        
        foreach (var newItem in e.NewItems?.OfType<IBall>() ?? [])
        {
            Balls.Add(new BallModel(newItem));
        }
    }

    /// <summary>
    /// Adds the balls.
    /// </summary>
    /// <param name="count">The count.</param>
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
