using System.Collections.ObjectModel;
using Data;

namespace Logic
{
    public class PoolController : ISimulationController
    {
        ITable _table;

        public event EventHandler<ReadOnlyCollection<IBall>> onBallUpdate;

        public PoolController(ITable table)
        {
            _table = table;
 
        }
        public void AddBall(IBall ball)
        {
            _table.AddBall(ball);
        }

        public void RemoveBalls()
        { 
            _table.ClearBalls();
        }
    }
}
