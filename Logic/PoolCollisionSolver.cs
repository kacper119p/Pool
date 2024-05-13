using System.Diagnostics;
using System.Numerics;
using Data;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Logic;

public class PoolCollisionSolver : ICollisionSolver
{
    private readonly ITable _table;
    private readonly Timer _timer;
    private readonly List<IBall> _locked;
    private readonly AabbTree _aabbTree = new AabbTree();

    public AabbTree CollisionTree => _aabbTree;

    public PoolCollisionSolver(ITable table, float interval)
    {
        _table = table;
        _locked = new List<IBall>(_table.BallCount);
        _timer = new Timer(interval);
        _timer.AutoReset = true;
        _timer.Elapsed += Tick;
        _timer.Start();
    }

    public void Update()
    {
        Tick(null, null);
    }

    private void Tick(Object? source, ElapsedEventArgs e)
    {
        _table.Lock.AcquireReaderLock(60000);
        _aabbTree.Lock.AcquireWriterLock(60000);
        _aabbTree.Clear();
        for (int i = 0; i < _table.BallCount; i++)
        {
            Monitor.Enter(_table.Balls[i].Lock);
            _locked.Add(_table.Balls[i]);
        }

        for (int i = 0; i < _locked.Count; i++)
        {
            IBall ball = _locked[i];
            Vector2 lowerBound = new Vector2(ball.Position.X - ball.Radius, ball.Position.Y - ball.Radius);
            Vector2 upperBound = new Vector2(ball.Position.X + ball.Radius, ball.Position.Y + ball.Radius);
            AabbBox bounds = new AabbBox(lowerBound, upperBound);
            _aabbTree.Insert(bounds, i);
        }

        foreach (IBall ball in _locked)
        {
            Monitor.Exit(ball.Lock);
        }
        _aabbTree.Lock.ReleaseWriterLock();
        _locked.Clear();
        _table.Lock.ReleaseReaderLock();
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
