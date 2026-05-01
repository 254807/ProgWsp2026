using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Model;

/// <summary>
/// Iball decorator/wrapper that auto-scales to window size.
/// </summary>
public sealed class BallModel : INotifyPropertyChanged
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BallModel"/> class.
    /// </summary>
    /// <param name="ball">The Iball instance</param>
    public BallModel(IBall ball)
    {
        DataBall = ball;

        ball.PropertyChanged += (sender, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(DataBall.Position):
                    BallChanged();
                    break;

            }
        };

        BallChanged();
    }
    
    public IBall DataBall { get; }

    /// <summary>
    /// Gets or sets the distance from the window's left border.
    /// </summary>
    public double Left 
    { 
        get; 
        set => SetField(ref field, value);
    }

    /// <summary>
    /// Gets or sets the distance from the window's top border.
    /// </summary>
    public double Top 
    { 
        get; 
        set => SetField(ref field, value);
    }

    /// <summary>
    /// Gets or sets the ball's diameter.
    /// </summary>
    public double Diameter
    {
        get;
        set => SetField(ref field, value);
    }
    
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// To be called when IBall fired <see langword='PropertyChanged'/> .
    /// </summary>
    private void BallChanged()
    {
        Left = DataBall.Position.X - DataBall.Radius;
        Top = DataBall.Position.Y - DataBall.Radius;
        Diameter = DataBall.Radius * 2;
    }

    /// <summary>
    /// Fires property changed event.
    /// </summary>
    /// <param name="propertyName">Name of changed property</param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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
        OnPropertyChanged(propertyName);
        return true;
    }
}
