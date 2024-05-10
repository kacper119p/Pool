using System.Numerics;
using Data;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Logic;

public class PoolCollisionSolver : ICollisionSolver
{
    private readonly ITable _table;
    private readonly float _interval;
    private readonly Timer _timer;
    private readonly List<IBall> _locked;

    public PoolCollisionSolver(ITable table, float interval)
    {
        _table = table;
        _interval = interval;
        _locked = new List<IBall>(_table.BallCount);
        _timer = new Timer(interval);
        _timer.AutoReset = true;
        _timer.Elapsed += Tick;
        _timer.Start();
    }

    private void Tick(Object? source, ElapsedEventArgs e)
    {
        lock (_table.Lock)
        {
            for (int i = 0; i < _table.BallCount; i++)
            {
                Monitor.Enter(_table.Balls[i].Lock);
                _locked.Add(_table.Balls[i]);
            }

            SolveCollisions(_locked);

            foreach (IBall ball in _locked)
            {
                Monitor.Exit(ball.Lock);
            }

            _locked.Clear();
        }
    }

    private void SolveCollisions(List<IBall> balls)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                if (CheckCollision(balls[i], balls[j]))
                {
                    SolveCollision(balls[i], balls[j]);
                }
            }
            BoundsCollision(balls[i]);
        }
    }

    private void SolveCollision(IBall a, IBall b)
    {
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
