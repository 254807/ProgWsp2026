using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

/// <summary>
/// Ball logic.
/// </summary>
public interface IBallLogic : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the balls.
    /// </summary>
    ReadOnlyObservableCollection<IBall> Balls { get; }

    /// <summary>
    /// Gets or sets the bounds.
    /// </summary>
    Rectangle Bounds { get; set; }

    /// <summary>
    /// Adds a ball.
    /// </summary>
    /// <param name="cancellationToken">Token which cancels the movement of the new ball.</param>
    void AddBall(CancellationToken cancellationToken);
    
    /// <summary>
    /// Adds given number of balls.
    /// </summary>
    /// <param name="ballCount">The ball count.</param>
    IReadOnlyList<CancellationTokenSource> AddBalls(int ballCount);
}
