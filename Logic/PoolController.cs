using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Timers;
using Data;
using Data.Logging;
using Timer = System.Timers.Timer;

namespace Logic
{
    public class PoolController : ISimulationController
    {
        private const float UpdateInterval = 16.666666666666f;

        private readonly ITable _table;
        private readonly IBallsBehaviourFactory _ballsBehaviourFactory;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly object _tablesLock = new object();
        private readonly List<PoolBallData> _ballData = new List<PoolBallData>();
        private readonly List<IPoolBallsBehaviour> _behaviourHandles = new List<IPoolBallsBehaviour>();
        private readonly Random _random = new Random();
        private readonly Timer _updateTimer;
        private readonly ICollisionSolver _collisionSolver;
        private readonly ILogger _logger;

        public object Lock => _tablesLock;

        public event EventHandler<ReadOnlyCollection<IBallData>>? OnBallsUpdate;

        public PoolController(ITable table, IBallsBehaviourFactory ballsBehaviourFactory,
            ICollisionSolverFactory collisionSolverFactory, ILogger logger)
        {
            _table = table;
            _ballsBehaviourFactory = ballsBehaviourFactory;
            _updateTimer = new Timer(UpdateInterval);
            _updateTimer.AutoReset = true;
            _updateTimer.Elapsed += Update;
            _updateTimer.Start();
            _collisionSolver = collisionSolverFactory.Create(table, UpdateInterval);
            _logger = logger;
        }

        public void AddBall(Color color, Vector2 position, Vector2 velocity, float mass, float radius)
        {
            lock (_tablesLock)
            {
                _table.Lock.AcquireWriterLock(60000);
                PoolBall ball = new PoolBall(color, position, velocity, mass, radius);
                _table.AddBall(ball);
                _behaviourHandles.Add(_ballsBehaviourFactory.Create(ball, UpdateInterval,
                    _collisionSolver.CollisionTree, _behaviourHandles.Count, _table, _logger));
                _ballData.Add(new PoolBallData());
                _table.Lock.ReleaseWriterLock();
            }
        }

        public void AddBall(IBall ball)
        {
            lock (_tablesLock)
            {
                _table.Lock.AcquireWriterLock(60000);
                _table.AddBall(ball);
                _behaviourHandles.Add(_ballsBehaviourFactory.Create(ball, UpdateInterval,
                    _collisionSolver.CollisionTree, _behaviourHandles.Count, _table, _logger));
                _ballData.Add(new PoolBallData());
                _table.Lock.ReleaseWriterLock();
            }
        }

        public void RemoveBalls()
        {
            lock (_tablesLock)
            {
                _table.Lock.AcquireWriterLock(60000);
                foreach (var behaviour in _behaviourHandles)
                {
                    behaviour.Dispose();
                }

                _behaviourHandles.Clear();
                _table.ClearBalls();
                _ballData.Clear();
                _table.Lock.ReleaseWriterLock();
            }
        }

        private void Update(Object? source, ElapsedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            float previousTime = (float)stopwatch.Elapsed.TotalSeconds;
            lock (_tablesLock)
            {
                ReadOnlyCollection<IBall> balls = _table.Balls;
                for (int i = 0; i < _ballData.Count; i++)
                {
                    lock (balls[i].Lock)
                    {
                        _ballData[i].Color = balls[i].Color;
                        _ballData[i].Position = balls[i].Position;
                        _ballData[i].Radius = balls[i].Radius;
                    }
                }

                OnBallsUpdate?.Invoke(this, _ballData.Cast<IBallData>().ToList().AsReadOnly());
            }
        }

        public void Dispose()
        {
            lock (_tablesLock)
            {
                _updateTimer.Stop();
                _updateTimer.Dispose();
                _collisionSolver.Dispose();
                RemoveBalls();
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _logger.Dispose();
            }
        }

        public void RemoveBalls(int amount)
        {
            lock (_tablesLock)
            {
                _table.Lock.AcquireWriterLock(60000);
                for (int i = 0; i < _table.BallCount; i++)
                {
                    Monitor.Enter(_table.Balls[i]);
                }

                amount = Math.Min(_ballData.Count, amount);
                for (int i = 0; i < amount; i++)
                {
                    int index = _random.Next(0, _table.BallCount);
                    _table.RemoveAt(index);
                    _behaviourHandles[index].Dispose();
                    _behaviourHandles.RemoveAt(index);
                    _ballData.RemoveAt(_ballData.Count - 1);
                }

                for (int i = 0; i < _behaviourHandles.Count; i++)
                {
                    _behaviourHandles[i].Id = i;
                }

                _collisionSolver.Update();

                for (int i = 0; i < _table.BallCount; i++)
                {
                    Monitor.Exit(_table.Balls[i]);
                }
                _table.Lock.ReleaseWriterLock();
            }
        }
    }
}
