namespace ConcurrentProgramming.Data;

/// <summary>
/// Represents a two-dimensional vector.
/// </summary>
public record struct Vector(double X, double Y)
{
    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="a">The first vector to add</param>
    /// <param name="b">The second vector to add</param>
    /// <returns>A new <see cref="Vector"/> that is the sum of <paramref name="a"/> and <paramref name="b"/></returns>
    public static Vector operator+(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);

    /// <summary>
    /// Subtracts one vector from another.
    /// </summary>
    /// <param name="a">The vector to subtract from</param>
    /// <param name="b">The vector to subtract</param>
    /// <returns>A new <see cref="Vector"/> representing the difference between <paramref name="a"/> and <paramref name="b"/></returns>
    public static Vector operator-(Vector a, Vector b) => new(a.X - b.X, a.Y - b.Y);

    /// <summary>
    /// Multiplies a vector by a scalar value.
    /// </summary>
    /// <param name="a">The vector to scale</param>
    /// <param name="b">The scalar multiplier</param>
    /// <returns>A new <see cref="Vector"/> where each component is multiplied by <paramref name="b"/></returns>
    public static Vector operator*(Vector a, double b) => new(a.X * b, a.Y * b);
}
