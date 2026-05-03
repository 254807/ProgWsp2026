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
    public async Task AddBallsTests()
    {
        BallLogic ballLogic = new();
        const int numberOfBalls = 10;

        ballLogic.AddBalls(numberOfBalls);

        await Task.Delay(1000);
        Assert.Equal(numberOfBalls, ballLogic.Balls.Count);

        ballLogic.RemoveBalls(numberOfBalls);
        
        await Task.Delay(1000);
        Assert.Empty(ballLogic.Balls);
    }

    /// <summary>
    /// Bounds change tests.
    /// </summary>
    [Fact]
    public async Task BoundsTests()
    {
        BallLogic ballLogic = new();
        ballLogic.Bounds = ballLogic.Bounds with { X = 10, Y = 10 };

        Assert.Equal(10, ballLogic.Bounds.X);
        Assert.Equal(10, ballLogic.Bounds.Y);
    }
}
