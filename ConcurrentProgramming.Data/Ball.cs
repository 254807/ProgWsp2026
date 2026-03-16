using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConcurrentProgramming.Data;

public sealed class Ball : IBall
{
    public Ball(Vector position, Vector velocity)
    {
        Position = position;
        Velocity = velocity;
    }

    public Vector Position
    {
        get;
        set => SetField(ref field, value);
    }

    public Vector Velocity
    {
        get;
        set => SetField(ref field, value);
    }
        
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}