using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using Data;

namespace Logic
{
    public class PoolController
    {
        public event EventHandler<ReadOnlyCollection<Ball>> SendBalls;
        public void AddBall(Ball ball, TableApi tableApi)
        {
            tableApi.Balls.Add(ball);
        }

        public void RemoveBalls(TableApi tableApi)
        {
            tableApi.Balls.Clear();
        }

        protected virtual void SendTable(TableApi tableApi)
        {
            SendBalls.Invoke(this,tableApi.Balls.AsReadOnly());
        }
    }
}
