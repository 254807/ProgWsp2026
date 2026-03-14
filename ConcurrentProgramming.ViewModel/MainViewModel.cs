using ConcurrentProgramming.Data;
using ConcurrentProgramming.Logic;
using System.Collections.ObjectModel;

namespace ConcurrentProgramming.ViewModel
{
    public class MainViewModel
    {
        public readonly ObservableCollection<Ball> Balls = [];

        public void AddBalls(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Balls.Add(Utils.CreateBall());
            }
        }
    }
}
