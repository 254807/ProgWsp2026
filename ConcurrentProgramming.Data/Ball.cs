using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConcurrentProgramming.Data;

/// <summary>
/// Standard ball implementation.
/// </summary>
public sealed class Ball : IBall
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Ball"/> class.
    /// </summary>
    /// <param name="position">The position</param>
    /// <param name="velocity">The velocity</param>
    public Ball(Vector position, Vector velocity)
    {
        Position = position;
        Velocity = velocity;
    }

    /// <inheritdoc/>
    public Vector Position
    {
        get;
        set => SetField(ref field, value);
    }

    /// <inheritdoc/>
    public Vector Velocity
    {
        get;
        set => SetField(ref field, value);
    }

    /// <inheritdoc/>
    public double Radius => 5;

    /// <inheritdoc/>
    public double Weight => Radius * 2;

    /// <inheritdoc/>
    public void Move(TimeSpan elapsed)
    {
        Position += Velocity * elapsed.TotalSeconds;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Fires property changed event.
    /// </summary>
    /// <param name="propertyName">Name of changed property</param>
    private void FirePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Sets field.
    /// </summary>
    /// <param name="field">Field reference</param>
    /// <param name="value">New value</param>
    /// <param name="propertyName">Property name</param>
    /// <returns>True if change accually occured, false otherwise</returns>
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        FirePropertyChanged(propertyName);
        return true;
    }
}
