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
    void AddBall();
    
    /// <summary>
    /// Adds given number of balls.
    /// </summary>
    /// <param name="ballCount">The ball count.</param>
    void AddBalls(int ballCount);

    /// <summary>
    /// Removes the given number of balls.
    /// </summary>
    /// <param name="ballCount">The ball count.</param>
    void RemoveBalls(int ballCount);
}
