using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

/// <summary>
/// Ball logic.
/// </summary>
public interface IBallLogic
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
    /// Runs the ball moving loop asynchronously infinitely.
    /// </summary>
    /// <returns>A Task that runs the loop</returns>
    Task RunMainLoop();

    /// <summary>
    /// Adds given number of balls.
    /// </summary>
    /// <param name="ballCount">The ball count.</param>
    void AddBalls(int ballCount);
}
