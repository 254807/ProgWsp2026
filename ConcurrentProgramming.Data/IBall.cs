using System.ComponentModel;

namespace ConcurrentProgramming.Data;

public interface IBall : INotifyPropertyChanged
{
    Vector Position { get; set; }
    
    Vector Velocity { get; set; }
    
    double Radius { get; }
}
