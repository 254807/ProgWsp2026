using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

/// <summary>
/// Standard ball logic.
/// </summary>
public sealed class BallLogic : IBallLogic
{
    private readonly ObservableCollection<IBall> _balls = [];

    private readonly Random _random = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="BallLogic"/> class.
    /// </summary>
    public BallLogic()
    {
        Balls = new(_balls);
    }

    /// <inheritdoc />
    public ReadOnlyObservableCollection<IBall> Balls { get; }
    
    /// <inheritdoc />
    public void AddBalls(int ballCount)
    {
        for (var i = 0; i < ballCount; i++)
        {
            var ball = CreateBall();
            _balls.Add(ball);
            ball.PropertyChanged += BallOnPropertyChanged;
        }
    }

    /// <inheritdoc />
    public Rectangle Bounds
    {
        get;
        set => SetField(ref field, value);
    } = new Rectangle(0, 0, 1920 / 6, 1080 / 6);

    /// <inheritdoc />
    public async Task RunMainLoop()
    {
        var timestamp = Stopwatch.GetTimestamp();
        
        while (true)
        {
            var elapsed = Stopwatch.GetElapsedTime(timestamp);
            timestamp = Stopwatch.GetTimestamp();
            
            foreach (var ball in _balls)
            {
                ball.Move(elapsed);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(1.0 / 60.0));
        }
    }
    
    private void BallOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not IBall ball || e.PropertyName != nameof(IBall.Position)) return;
        
        var newPosition = ball.Position;
        if (newPosition.Y + ball.Radius > Bounds.Bottom)
        {
            ball.Velocity = ball.Velocity with { Y = -ball.Velocity.Y };
            ball.Position = ball.Position with { Y = Bounds.Bottom - ball.Radius };
        }
        else if (newPosition.Y - ball.Radius < Bounds.Top)
        {
            ball.Velocity = ball.Velocity with { Y = -ball.Velocity.Y };
            ball.Position = ball.Position with { Y = Bounds.Top + ball.Radius };
        }
        
        if (newPosition.X + ball.Radius > Bounds.Right)
        {
            ball.Velocity = ball.Velocity with { X = -ball.Velocity.X };
            ball.Position = ball.Position with { X = Bounds.Right - ball.Radius };
        }
        
        // ReSharper disable once InvertIf
        else if (newPosition.X - ball.Radius < Bounds.Left)
        {
            ball.Velocity = ball.Velocity with { X = -ball.Velocity.X };
            ball.Position = ball.Position with { X = Bounds.Left + ball.Radius };
        }
    }

    /// <summary>
    /// Creates a new initialized standard ball.
    /// </summary>
    /// <returns>A newly created Ball</returns>
    private Ball CreateBall()
    {
        var velocity = new Vector(_random.NextDouble() - 0.5, _random.NextDouble() -0.5) * 90;
        velocity.X += double.Sign(velocity.X);
        velocity.Y += double.Sign(velocity.Y);
        
        var ball = new Ball(
            new Vector(_random.NextDouble() * Bounds.Width, _random.NextDouble() * Bounds.Height),
            velocity
        );

        ball.Position = new Vector(
            double.Clamp(ball.Position.X, ball.Radius, Bounds.Width - ball.Radius),
            double.Clamp(ball.Position.Y, ball.Radius, Bounds.Height - ball.Radius)
        );

        return ball;
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
