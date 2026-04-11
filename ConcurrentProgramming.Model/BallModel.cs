using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Model;

/// <summary>
/// The ball model.
/// </summary>
public sealed class BallModel : INotifyPropertyChanged
{
    private readonly IBall _ball;

    /// <summary>
    /// Initializes a new instance of the <see cref="BallModel"/> class.
    /// </summary>
    /// <param name="ball">The ball</param>
    public BallModel(IBall ball)
    {
        _ball = ball;

        ball.PropertyChanged += (sender, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(_ball.Position):
                    BallChanged();
                    break;

            }
        };

        BallChanged();
    }

    /// <summary>
    /// Gets or sets the left side.
    /// </summary>
    public double Left 
    { 
        get; 
        set => SetField(ref field, value);
    }

    /// <summary>
    /// Gets or sets the top.
    /// </summary>
    public double Top 
    { 
        get; 
        set => SetField(ref field, value);
    }

    /// <summary>
    /// Gets or sets the diameter.
    /// </summary>
    public double Diameter
    {
        get;
        set => SetField(ref field, value);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Balls the changed.
    /// </summary>
    private void BallChanged()
    {
        Left = _ball.Position.X - _ball.Radius;
        Top = _ball.Position.Y - _ball.Radius;
        Diameter = _ball.Radius * 2;
    }

    /// <summary>
    /// Ons the property changed.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Sets the field.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="value">The value.</param>
    /// <param name="propertyName">The property name.</param>
    /// <returns>A bool.</returns>
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
