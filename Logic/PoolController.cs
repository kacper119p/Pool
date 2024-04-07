using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using Data;

namespace Logic
{
    public class PoolController
    {
        public event EventHandler<ReadOnlyCollection<Ball>> SendBalls;
        public void AddBall(Ball ball, Table table)
        {
            table.Balls.Add(ball);
        }

        public void RemoveBalls(Table table)
        {
            table.Balls.Clear();
        }

        protected virtual void SendTable(Table table)
        {
            SendBalls.Invoke(this,table.Balls.AsReadOnly());
        }
    }
}
