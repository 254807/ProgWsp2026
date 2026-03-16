using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Model;

public sealed class BallModel : INotifyPropertyChanged
{
    private readonly IBall _ball;

    public BallModel(IBall ball)
    {
        _ball = ball;

        BallPositionChanged();
        ball.PropertyChanged += BallOnPropertyChanged;
    }

    public double Left { 
        get; 
        set => SetField(ref field, value);
    }
    
    public double Top { 
        get; 
        set => SetField(ref field, value);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void BallOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_ball.Position):
                BallPositionChanged();
                break;
            
        }
    }

    private void BallPositionChanged()
    {
        Left = _ball.Position.X;
        Top = _ball.Position.Y;
    }

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