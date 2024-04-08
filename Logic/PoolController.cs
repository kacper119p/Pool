using System.Collections.ObjectModel;
using Data;

namespace Logic
{
    public class PoolController : ISimulationController
    {
        ITable _table;
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public event EventHandler<ReadOnlyCollection<IBall>> OnBallsUpdate;

        public PoolController(ITable table)
        {
            _table = table;
            TaskFactory taskFactory = new TaskFactory(_cancellationTokenSource.Token);
            taskFactory.StartNew(Update, _cancellationTokenSource.Token);
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
            while (true)
            {
                OnBallsUpdate.Invoke(this, _table.Balls);
                await Task.Yield();
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
