using ConcurrentProgramming.Data;
using System;

namespace ConcurrentProgramming.Logic
{
    public static class Utils
    {
        private static readonly Random random = new();

        public static Ball CreateBall()
        {
            return new Ball(random.NextDouble(), random.NextDouble());
        }
    }
}
