using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentProgramming.Data;

/// <summary>
/// Standard ball implementation.
/// </summary>
public sealed class Ball : IBall
{
    private readonly Lock _lock = new(); // Lock object for thread safety

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

    private Vector _position;
    
    /// <inheritdoc/>
    public Vector Position
    {
        get
        {
            lock (_lock)
            {
                return _position;
            }
        }
        set => SetField(ref _position, value);
    }

    /// <inheritdoc/>
    public Vector Velocity
    {
        get
        {
            lock (_lock)
            {
                return field;
            }
        }
        set => SetField(ref field, value);
    }

    /// <inheritdoc/>
    public double Radius => 5;

    /// <inheritdoc/>
    public double Mass => Radius * Radius * 0.2;

    /// <inheritdoc/>
    public async Task Move(CancellationToken cancellationToken)
    {
        var timestamp = Stopwatch.GetTimestamp();

        while (!cancellationToken.IsCancellationRequested)
        {
            var elapsed = Stopwatch.GetElapsedTime(timestamp);
            timestamp = Stopwatch.GetTimestamp();

            // We want to increment Position by Velocity atomically,
            // but don't want to trigger the PropertyChanged event inside the lock to prevent deadlocks,
            // so we'll have to write to the backing field and fire the event manually here.
            lock (_lock)
            {
                _position += Velocity * elapsed.TotalSeconds;
            }
            FirePropertyChanged(nameof(Position));

            await Task.Delay(TimeSpan.FromSeconds(1.0 / 60.0), cancellationToken);
        }
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
    /// <returns>True if change actually occured, false otherwise</returns>
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        lock (_lock)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
        }

        FirePropertyChanged(propertyName);
        return true;
    }
}
