using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public sealed class BallLogic : IBallLogic
{
    public ObservableCollection<IBall> Balls { get; } = [];

    public Rectangle Bounds { get; set; } = new Rectangle(0, 0, 1920 / 6, 1080 / 6);
    
    private readonly Random _random = new();
    
    public async Task RunMainLoop()
    {
        var timestamp = Stopwatch.GetTimestamp();
        
        while (true)
        {
            var elapsed = Stopwatch.GetElapsedTime(timestamp);
            timestamp = Stopwatch.GetTimestamp();
            
            foreach (var ball in Balls)
            {
                MoveBall(ball, elapsed);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(1.0 / 60.0));
        }
    }

    private void MoveBall(IBall ball, TimeSpan elapsed)
    {
        var newPosition = ball.Position + ball.Velocity * elapsed.TotalSeconds;

        if (newPosition.Y + ball.Radius > Bounds.Bottom || newPosition.Y - ball.Radius < Bounds.Top)
        {
            ball.Velocity = ball.Velocity with { Y = -ball.Velocity.Y };
            return;
        }
        
        if (newPosition.X - ball.Radius < Bounds.Left || newPosition.X + ball.Radius > Bounds.Right)
        {
            ball.Velocity = ball.Velocity with { X = -ball.Velocity.X };
            return;
        }
        
        ball.Position = newPosition;
    }

    public void AddBalls(int ballCount)
    {
        for (var i = 0; i < ballCount; i++)
        {
            Balls.Add(CreateBall());
        }
    }

    private IBall CreateBall()
    {
        var velocity = new Vector(_random.NextDouble() - 0.5, _random.NextDouble() -0.5) * 60;
        velocity.X += 1 * double.Sign(velocity.X);
        velocity.Y += 1 * double.Sign(velocity.Y);
        
        return new Ball(
            new Vector(_random.NextDouble() * Bounds.Width, _random.NextDouble() * Bounds.Height),
            velocity
        );
    }
}
