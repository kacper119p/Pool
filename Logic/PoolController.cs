using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using Data;

namespace Logic
{
    public class PoolController : ISimulationController
    {
        private const float MinFrameDuration = 0.011f;

        private readonly ITable _table;
        private readonly IPoolBallsBehaviour _ballsBehaviour;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _updateHandle;
        private readonly object _tablesLock = new object();
        private readonly List<PoolBallData> _ballData = new List<PoolBallData>();

        public object Lock => _tablesLock;

        public event EventHandler<ReadOnlyCollection<IBallData>>? OnBallsUpdate;

        public PoolController(ITable table, IPoolBallsBehaviour ballsBehaviour)
        {
            _table = table;
            _ballsBehaviour = ballsBehaviour;
            _updateHandle = Task.Run(() => { Update(_cancellationTokenSource.Token); });
        }

        public void AddBall(Color color, Vector2 position, Vector2 velocity, float mass, float radius)
        {
            lock (_tablesLock)
            {
                _table.AddBall(new PoolBall(color, position, velocity, mass, radius));
                _ballData.Add(new PoolBallData());
            }
        }

        public void AddBall(IBall ball)
        {
            lock (_tablesLock)
            {
                _table.AddBall(ball);
                _ballData.Add(new PoolBallData());
            }
        }

        public void RemoveBalls()
        {
            lock (_tablesLock)
            {
                _table.ClearBalls();
                _ballData.Clear();
            }
        }

        private void Update(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            float previousTime = (float)stopwatch.Elapsed.TotalSeconds;
            while (!cancellationToken.IsCancellationRequested)
            {
                lock (_tablesLock)
                {
                    float currentTime;
                    do
                    {
                        currentTime = (float)stopwatch.Elapsed.TotalSeconds;
                    } while (currentTime - previousTime < MinFrameDuration);

                    _ballsBehaviour.Tick(currentTime - previousTime, _table);
                    ReadOnlyCollection<IBall> balls = _table.Balls;
                    for (int i = 0; i < _ballData.Count; i++)
                    {
                        _ballData[i].Color = balls[i].Color;
                        _ballData[i].Position = balls[i].Position;
                        _ballData[i].Radius = balls[i].Radius;
                    }
                    OnBallsUpdate?.Invoke(this, _ballData.Cast<IBallData>().ToList().AsReadOnly());
                    previousTime = currentTime;
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
