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
}
