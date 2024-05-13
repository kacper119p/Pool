using System.Numerics;
using System.Timers;
using Data;
using Timer = System.Timers.Timer;

namespace Logic;

public class PoolBallsBehaviour : IPoolBallsBehaviour
{
    private readonly Timer _timer;
    private readonly IBall _ball;
    private readonly float _deltaTime;
    private readonly AabbTree _collisionTree;
    private readonly ITable _table;
    private int _id;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public PoolBallsBehaviour(IBall ball, float interval, AabbTree collisionTree, int id, ITable table)
    {
        _id = id;
        _ball = ball;
        _table = table;
        _collisionTree = collisionTree;
        _timer = new Timer(interval);
        _deltaTime = interval / 1000.0f;
        _timer.AutoReset = true;
        _timer.Elapsed += Tick;
        _timer.Enabled = true;
    }

    private void Tick(Object? source, ElapsedEventArgs e)
    {
        _table.Lock.AcquireReaderLock(60000);
        _collisionTree.Lock.AcquireReaderLock(60000);
        lock (_ball.Lock)
        {
            _ball.Position += _ball.Velocity * _deltaTime;

            Vector2 lowerBound = new Vector2(_ball.Position.X - _ball.Radius, _ball.Position.Y - _ball.Radius);
            Vector2 upperBound = new Vector2(_ball.Position.X + _ball.Radius, _ball.Position.Y + _ball.Radius);
            AabbBox bounds = new AabbBox(lowerBound, upperBound);

            LinkedList<int> candidates = _collisionTree.Query(bounds);
            foreach (int candidate in candidates)
            {
                if (candidate > _id)
                {
                    lock (_table.Balls[candidate])
                    {
                        SolveCollision(_ball, _table.Balls[candidate]);
                    }
                }
            }

            BoundsCollision(_ball);
        }
        _collisionTree.Lock.ReleaseReaderLock();
        _table.Lock.ReleaseReaderLock();
    }

    private void SolveCollision(IBall a, IBall b)
    {
        if (!CheckCollision(a, b))
        {
            return;
        }

        Vector2 offset = a.Position - b.Position;
        float overlay = (a.Radius + b.Radius) - offset.Length();
        offset = offset / offset.Length() * overlay;
        a.Position += offset;
        b.Position -= offset;

        Vector2 v1 = a.Velocity;
        a.Velocity = new Vector2(
            (a.Velocity.X * (a.Mass - b.Mass) + 2 * b.Mass * b.Velocity.X) / (a.Mass + b.Mass),
            (a.Velocity.Y * (a.Mass - b.Mass) + 2 * b.Mass * b.Velocity.Y) / (a.Mass + b.Mass));
        b.Velocity = new Vector2(
            (b.Velocity.X * (b.Mass - a.Mass) + 2 * a.Mass * v1.X) / (a.Mass + b.Mass),
            (b.Velocity.Y * (b.Mass - a.Mass) + 2 * a.Mass * v1.Y) / (a.Mass + b.Mass));
    }

    private bool CheckCollision(IBall a, IBall b)
        => (a.Position - b.Position).Length() <= a.Radius + b.Radius;

    private void BoundsCollision(IBall ball)
    {
        if ((ball.Position.X - ball.Radius) < 0)
        {
            ball.Position = new Vector2(ball.Radius, ball.Position.Y);
            ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
        }
        else if ((ball.Position.X + ball.Radius) > _table.SizeX)
        {
            ball.Position = new Vector2(_table.SizeX - ball.Radius, ball.Position.Y);
            ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
        }

        if ((ball.Position.Y - ball.Radius) < 0)
        {
            ball.Position = new Vector2(ball.Position.X, ball.Radius);
            ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
        }
        else if ((ball.Position.Y + ball.Radius) > _table.SizeY)
        {
            ball.Position = new Vector2(ball.Position.X, _table.SizeY - ball.Radius);
            ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
        }
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
