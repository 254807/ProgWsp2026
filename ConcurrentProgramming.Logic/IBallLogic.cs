using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public interface IBallLogic
{
    ObservableCollection<IBall> Balls { get; }
    
    Rectangle Bounds { get; set; }
    
    Task RunMainLoop();
    
    void AddBalls(int ballCount);
}
