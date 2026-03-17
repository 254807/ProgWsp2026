using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public sealed class BallLogic : IBallLogic
{
    public ObservableCollection<IBall> Balls { get; } = [];
    
    private readonly Lock _lock = new(); 
    
    private readonly Random _random = new();
    
    public async Task RunMainLoop(CancellationToken cancellationToken)
    {
        var timestamp = Stopwatch.GetTimestamp();
        while (!cancellationToken.IsCancellationRequested)
        {
            var elapsed = Stopwatch.GetElapsedTime(timestamp);
            timestamp = Stopwatch.GetTimestamp();
            lock (_lock)
            {
                foreach (var ball in Balls)
                {
                    ball.Position += ball.Velocity * elapsed.TotalSeconds;
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
        return new Ball(
            new Vector(_random.NextDouble(), _random.NextDouble()) * 12.0, 
            new Vector(_random.NextDouble(), _random.NextDouble()) * 60
        );
    }
}