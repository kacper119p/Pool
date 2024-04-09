using Data;

namespace Logic;

internal class PoolBallController : IBallController
{
    private readonly IBall _ball;

    public PoolBallController(IBall ball, ITable table)
    {
        _ball = ball;
    }

    public void Tick(float deltaTime)
    {
        _ball.Position += _ball.Velocity * deltaTime;
    }
}
