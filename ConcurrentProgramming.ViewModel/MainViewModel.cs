using System;
using ConcurrentProgramming.Data;
using ConcurrentProgramming.Logic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using ConcurrentProgramming.Model;

namespace ConcurrentProgramming.ViewModel;

public sealed class MainViewModel
{
    public IBallLogic BallLogic { get; set; }

    public ObservableCollection<BallModel> Balls { get; } = [];
    
    public MainViewModel()
    {
        BallLogic = new BallLogic();
        BallLogic.Balls.CollectionChanged += BallsOnCollectionChanged;
        
        AddBalls(12);

        BallLogic.RunMainLoop();
        
        ArgumentNullException.ThrowIfNull(Application.Current.MainWindow);
        Application.Current.MainWindow.SizeChanged += MainWindowOnSizeChanged;
    }

    private void MainWindowOnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        var scaleX = e.NewSize.Width / BallLogic.Bounds.Width;
        var scaleY = e.NewSize.Height / BallLogic.Bounds.Height;
        var scale = double.Min(scaleX, scaleY);
        
        foreach (var b in Balls)
        {
            b.Scale = scale;
        }
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
}