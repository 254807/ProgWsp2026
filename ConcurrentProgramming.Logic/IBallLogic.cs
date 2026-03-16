using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public interface IBallLogic
{
    ObservableCollection<IBall> Balls { get; }
    
    Task RunMainLoop(CancellationToken cancellationToken);
    
    void AddBalls(int ballCount);
}