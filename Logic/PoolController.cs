using System.Collections.ObjectModel;
using System.Diagnostics;
using Data;

namespace Logic
{
    public class PoolController : ISimulationController
    {
        ITable _table;
        private PoolBallController _poolBallController;
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public event EventHandler<ReadOnlyCollection<IBall>>? OnBallsUpdate;

        public PoolController(ITable table)
        {
            _table = table;
            _poolBallController = new PoolBallController(_table);
            Task.Run(() => { _ = Update(_cancellationTokenSource.Token); });
        }
        public void AddBall(IBall ball)
        {
            _table.AddBall(ball);
        }

        public void RemoveBalls()
        {
            _table.ClearBalls();
        }

        private async Task Update(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            float previousTime = stopwatch.ElapsedMilliseconds / 1000.0f;
            while (!cancellationToken.IsCancellationRequested)
            {
                float currentTime = stopwatch.ElapsedMilliseconds / 1000.0f;
                OnBallsUpdate?.Invoke(this, _table.Balls);
                _poolBallController.Tick(currentTime-previousTime);
                previousTime = currentTime;
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
