using System.Collections.ObjectModel;
using Data;

namespace Logic
{
    public class PoolController : ISimulationController
    {
        ITable _table;
        bool _isDisposed;

        public event EventHandler<ReadOnlyCollection<IBall>> OnBallsUpdate;

        public PoolController(ITable table)
        {
            _table = table;
            _ = Task.Run(Update);
        }
        public void AddBall(IBall ball)
        {
            _table.AddBall(ball);
        }

        public void RemoveBalls()
        {
            _table.ClearBalls();
        }

        private async Task Update()
        {
            while (!_isDisposed)
            {
                OnBallsUpdate.Invoke(this, _table.Balls);
                await Task.Yield();
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
