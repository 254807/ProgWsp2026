using System;
using ConcurrentProgramming.Data;
using ConcurrentProgramming.Logic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
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

        BallLogic.RunMainLoop(CancellationToken.None);
    }

    private void BallsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var newItem in e.NewItems.OfType<IBall>())
            {
                Balls.Add(new BallModel(newItem));
            }
        }
    }

    public void AddBalls(int count)
    {
        BallLogic.AddBalls(count);
    }
}