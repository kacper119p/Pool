using System.Numerics;
using System.Timers;
using Data;
using Timer = System.Timers.Timer;

namespace Logic;

public class PoolBallsBehaviour : IPoolBallsBehaviour
{
    private readonly Timer _timer;
    private readonly IBall _ball;
    private readonly ITable _table;
    private readonly float deltaTime;

    public PoolBallsBehaviour(IBall ball, ITable table, float interval)
    {
        _ball = ball;
        _table = table;
        _timer = new Timer(interval);
        deltaTime = (float)interval / 1000.0f;
        _timer.AutoReset = true;
        _timer.Elapsed += Tick;
        _timer.Enabled = true;
    }

    private void Tick(Object? source, ElapsedEventArgs e)
    {
        _ball.Position += _ball.Velocity * (float)_timer.Interval / 1000.0f;
        if ((_ball.Position.X - _ball.Radius) < 0 && _ball.Velocity.X < 0)
        {
            _ball.Position = new Vector2(_ball.Radius, _ball.Position.Y);
            _ball.Velocity = new Vector2(-_ball.Velocity.X, _ball.Velocity.Y);
        }
        else if ((_ball.Position.X + _ball.Radius) > _table.SizeX && _ball.Velocity.X > 0)
        {
            _ball.Position = new Vector2(_table.SizeX - _ball.Radius, _ball.Position.Y);
            _ball.Velocity = new Vector2(-_ball.Velocity.X, _ball.Velocity.Y);
        }

        if ((_ball.Position.Y - _ball.Radius) < 0 && _ball.Velocity.Y < 0)
        {
            _ball.Position = new Vector2(_ball.Position.X, _ball.Radius);
            _ball.Velocity = new Vector2(_ball.Velocity.X, -_ball.Velocity.Y);
        }
        else if ((_ball.Position.Y + _ball.Radius) > _table.SizeY && _ball.Velocity.Y > 0)
        {
            _ball.Position = new Vector2(_ball.Position.X, _table.SizeY - _ball.Radius);
            _ball.Velocity = new Vector2(_ball.Velocity.X, -_ball.Velocity.Y);
        }
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
