using System.Collections.ObjectModel;
using Data;

namespace Logic
{
    public class PoolController
    {
        public event EventHandler<ReadOnlyCollection<Ball>> GetBalls;
        public void AddBall(Ball ball, TableApi table)
        {
            table.AddBall(ball);
        }

        public void RemoveBalls(TableApi table)
        { 
            table.ClearBalls();
        }

        protected virtual void SendTable(TableApi table)
        {
            GetBalls.Invoke(this,table.GetBalls());
        }
    }
}
