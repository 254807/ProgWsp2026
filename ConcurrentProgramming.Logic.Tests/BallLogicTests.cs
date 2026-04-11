namespace ConcurrentProgramming.Logic.Tests;

/// <summary>
/// The ball logic tests.
/// </summary>
public class BallLogicTests
{
    /// <summary>
    /// Adding balls tests.
    /// </summary>
    [Fact]
    public void AddBallsTests()
    {
        var ballLogic = new BallLogic();
        int numberOfBalls = 10;

        ballLogic.AddBalls(numberOfBalls);

        Assert.Equal(numberOfBalls, ballLogic.Balls.Count);
    }

    /// <summary>
    /// Runs the main loop tests.
    /// </summary>
    /// <returns>A Task.</returns>
    [Fact]
    public async Task RunMainLoopTests()
    {
        var ballLogic = new BallLogic();
        ballLogic.AddBalls(10);

        var task = ballLogic.RunMainLoop();

        await Task.Delay(10000);
        Assert.False(task.IsCompleted);
    }


    /// <summary>
    /// Tests Bounds change.
    /// </summary>
    [Fact]
    public void BoundsTests()
    {
        var ballLogic = new BallLogic();
        var oldBounds = ballLogic.Bounds;

        ballLogic.Bounds = new System.Drawing.Rectangle(0, 0, 0, 0);
        Assert.NotEqual(oldBounds, ballLogic.Bounds);
    }
}
