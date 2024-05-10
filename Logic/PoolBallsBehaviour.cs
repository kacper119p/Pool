using System.Numerics;
using System.Timers;
using Data;
using Timer = System.Timers.Timer;

namespace Logic;

public class PoolBallsBehaviour : IPoolBallsBehaviour
{
    private readonly Timer _timer;
    private readonly IBall _ball;
    private readonly float deltaTime;

    public PoolBallsBehaviour(IBall ball, float interval)
    {
        _ball = ball;
        _timer = new Timer(interval);
        deltaTime = (float)interval / 1000.0f;
        _timer.AutoReset = true;
        _timer.Elapsed += Tick;
        _timer.Enabled = true;
    }

    private void Tick(Object? source, ElapsedEventArgs e)
    {
        _ball.Position += _ball.Velocity * deltaTime;
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
