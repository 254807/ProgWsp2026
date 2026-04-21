using System;
using System.ComponentModel;

namespace ConcurrentProgramming.Data;

/// <summary>
/// Generic ball.
/// </summary>
public interface IBall : INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets ball's position.
    /// </summary>
    Vector Position { get; set; }

    /// <summary>
    /// Gets or sets ball's velocity.
    /// </summary>
    Vector Velocity { get; set; }

    /// <summary>
    /// Gets ball's radius.
    /// </summary>
    double Radius { get; }
    
    /// <summary>
    /// Gets the ball's weight.
    /// </summary>
    double Weight { get; }

    /// <summary>
    /// Moves the ball.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last frame.</param>
    void Move(TimeSpan elapsed);
}
