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
}
