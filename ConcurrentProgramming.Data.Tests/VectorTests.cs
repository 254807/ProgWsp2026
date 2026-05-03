namespace ConcurrentProgramming.Data.Tests;

/// <summary>
/// Vector tests.
/// </summary>
public class VectorTests
{
    private readonly Vector v1 = new(1, 0.02);
    private readonly Vector v2 = new(-0.1, 5);
    private readonly Vector v3 = new(-3, 4);
    private readonly Vector v4 = new(0, 0);

    /// <summary>
    /// Tests addition.
    /// </summary>
    [Fact]
    public void AddTests()
    {
        Assert.Equal(new Vector(0.9, 5.02), v1 + v2);
    }

    /// <summary>
    /// Tests subtraction.
    /// </summary>
    [Fact]
    public void SubtractTests()
    {
        Assert.Equal(new Vector(1.1, -4.98), v1 - v2);
    }

    /// <summary>
    /// Tests multiplication.
    /// </summary>
    [Fact]
    public void MultiplyTests()
    {
        Assert.Equal(new Vector(2, 0.04), v1 * 2);
        Assert.Equal(new Vector(0.1, -5), v2 * -1);
    }

    /// <summary>
    /// Tests normalization.
    /// </summary>
    [Fact]
    public void NormalizeTests()
    {
        Assert.Equal(new Vector(-0.6, 0.8), v3.Normalized());
        Assert.Equal(new Vector(0, 0), v4.Normalized());
    }

    /// <summary>
    /// Tests length methods.
    /// </summary>
    [Fact]
    public void LengthTests()
    {
        Assert.Equal(5, v3.Length());
        Assert.Equal(25, v3.LengthSquared());
    }

    /// <summary>
    /// Tests distance methods.
    /// </summary>
    [Fact]
    public void DistanceTests()
    {
        Assert.Equal(5, Vector.Distance(v3, v4));
        Assert.Equal(25, Vector.DistanceSquared(v3, v4));
    }

    /// <summary>
    /// Tests dot product method.
    /// </summary>
    [Fact]
    public void DotTests()
    {
        Assert.Equal(0, Vector.Dot(v3, v4));
        Assert.Equal(25, Vector.Dot(v3, v3));
    }
}
