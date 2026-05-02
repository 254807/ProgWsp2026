using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
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

    private readonly Lock _lock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="BallLogic"/> class.
    /// </summary>
    public BallLogic()
    {
        Balls = new ReadOnlyObservableCollection<IBall>(_balls);
    }

    /// <inheritdoc />
    public ReadOnlyObservableCollection<IBall> Balls { get; }

    private readonly Dictionary<IBall, CancellationTokenSource> _ballCancellationTokens = [];
    
    /// <inheritdoc />
    public void AddBall()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var ball = CreateBall();

        lock (_lock)
        {
            _ballCancellationTokens[ball] = cancellationTokenSource;
            _balls.Add(ball);
        }

        ball.PropertyChanged += BallOnPropertyChanged;
        _ = Task.Run(() => ball.Move(cancellationTokenSource.Token), cancellationTokenSource.Token);
    }

    /// <inheritdoc />
    public void AddBalls(int ballCount)
    {
        for (var i = 0; i < ballCount; i++)
        {
            AddBall();
        }
    }

    /// <inheritdoc />
    public void RemoveBalls(int ballCount)
    {
        lock (_lock)
        {
            for (var i = 0; i < ballCount; i++)
            {
                var last = _balls[^1];
                if (_ballCancellationTokens.Remove(last, out var cancellationTokenSource))
                {  
                    cancellationTokenSource.Cancel();
                }
                _balls.RemoveAt(_balls.Count - 1);
            }
        }
    }

    /// <inheritdoc />
    public Rectangle Bounds
    {
        get;
        set => SetField(ref field, value);
    } = new(0, 0, 1920 / 6, 1080 / 6);

    /// <summary>
    /// Handles the PropertyChanged event of a ball and performs collision and bounds checks.
    /// </summary>
    /// <param name="sender">The sender (expected to be an <see cref="IBall"/>).</param>
    /// <param name="e">Property changed event args.</param>
    private void BallOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not IBall ball || e.PropertyName != nameof(IBall.Position)) 
        { 
            return;
        }

        lock (_lock)
        {
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

            foreach (var otherBall in Balls)
            {
                if (ball == otherBall)
                {
                    continue;
                }

                var delta = otherBall.Position - ball.Position;

                if (delta.Length() >= ball.Radius + otherBall.Radius)
                {
                    continue;
                }

                var normal = delta.Normalized();
                var speed = Vector.Dot(ball.Velocity - otherBall.Velocity, normal);

                if (speed < 0)
                {
                    continue;
                }

                var impulse = normal * (2 * speed / (ball.Mass + otherBall.Mass));
                ball.Velocity -= impulse * otherBall.Mass;
                otherBall.Velocity += impulse * ball.Mass;
            }
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

    /// <summary>
    /// Event raised when a property on this instance changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">Name of the property that changed.</param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Sets a backing field and raises PropertyChanged when the value changes.
    /// </summary>
    /// <typeparam name="T">Field type.</typeparam>
    /// <param name="field">Reference to the field to set.</param>
    /// <param name="value">New value.</param>
    /// <param name="propertyName">Name of the property (automatically provided).</param>
    /// <returns>True if the field was changed; otherwise false.</returns>
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
