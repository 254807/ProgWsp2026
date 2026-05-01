using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

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
    /// Gets the ball's mass.
    /// </summary>
    double Mass { get; }

    /// <summary>
    /// Moves the ball.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token which can be used to cancel the movement thread.</param>
    Task Move(CancellationToken cancellationToken);
}
