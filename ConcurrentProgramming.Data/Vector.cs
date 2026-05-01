namespace ConcurrentProgramming.Data;

/// <summary>
/// Represents a two-dimensional vector.
/// </summary>
public record struct Vector(double X, double Y)
{
    /// <summary>
    /// Calculates the squared length of the vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    public readonly double LengthSquared() => X * X + Y * Y;

    /// <summary>
    /// Calculates the length (magnitude) of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    public readonly double Length() => double.Sqrt(LengthSquared());

    /// <summary>
    /// Returns a normalized version of the vector.
    /// </summary>
    /// <returns>A new <see cref="Vector"/> with a length of 1, or the default vector if the length is zero.</returns>
    public readonly Vector Normalized()
    {
        var length = Length();

        if (length == 0) { 
            return default;
        }

        return new Vector(X / length, Y / length);
    }

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

    /// <summary>
    /// Calculates the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The distance between <paramref name="value1"/> and <paramref name="value2"/>.</returns>
    public static double Distance(Vector value1, Vector value2)
    {
        return double.Sqrt(DistanceSquared(value1, value2));
    }

    /// <summary>
    /// Calculates the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between <paramref name="value1"/> and <paramref name="value2"/>.</returns>
    public static double DistanceSquared(Vector value1, Vector value2)
    {
        return (value1 - value2).LengthSquared();
    }

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The dot product of <paramref name="value1"/> and <paramref name="value2"/>.</returns>
    public static double Dot(Vector value1, Vector value2)
    {
        return value1.X * value2.X + value1.Y * value2.Y;
    }
}
