namespace ConcurrentProgramming.Data;

public record struct Vector(double X, double Y)
{
    public static Vector operator+(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);
    
    public static Vector operator-(Vector a, Vector b) => new(a.X - b.X, a.Y - b.Y);
        
    public static Vector operator*(Vector a, double b) => new(a.X * b, a.Y * b);
}