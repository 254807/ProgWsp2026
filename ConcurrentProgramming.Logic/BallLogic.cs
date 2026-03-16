using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentProgramming.Data;
using Vector = ConcurrentProgramming.Data.Vector;

namespace ConcurrentProgramming.Logic;

public sealed class BallLogic : IBallLogic
{
    public ObservableCollection<IBall> Balls { get; } = [];
    
    private readonly object _lock = new(); 
    
    private readonly Random _random = new Random();
    
    public async Task RunMainLoop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            lock (_lock)
            {
                foreach (var ball in Balls)
                {
                    ball.Position += ball.Velocity;
                }
            }
            
            await Task.Delay(TimeSpan.FromSeconds(1.0 / 60.0), cancellationToken);
        }
    }

    public void AddBalls(int ballCount)
    {
        lock (_lock)
        {
            for (var i = 0; i < ballCount; i++)
            {
                Balls.Add(CreateBall());
            }
        }
    }

    private IBall CreateBall()
    {
        var pos = new Vector(_random.NextDouble(), _random.NextDouble()) * 12.0;
        var velocity = new Vector(_random.NextDouble(), _random.NextDouble());
        
        return new Ball(pos, velocity);
    }
}