using System.Numerics;
using Data;

namespace Logic;

internal class PoolBallController
{

    private ITable _table;
    public PoolBallController(ITable table)
    {
        _table = table;
    }

    public void Tick(float deltaTime)
    {
            foreach (IBall ball in _table.Balls)
            {
                ball.Position += ball.Velocity * deltaTime;
                    if ((ball.Position.X - ball.Radius) <= 0) { 
                        ball.Position = new Vector2(ball.Radius, ball.Position.Y);
                        ball.Velocity = Vector2.Zero;
                    }
                    else if ((ball.Position.X - ball.Radius) >= _table.SizeX)
                    {
                        ball.Position = new Vector2(_table.SizeX - ball.Radius, ball.Position.Y);
                        ball.Velocity = Vector2.Zero;

                    }

                    if ((ball.Position.Y - ball.Radius) <= 0)
                    {
                        ball.Position = new Vector2(ball.Position.X, ball.Radius);
                        ball.Velocity = Vector2.Zero;
                    }
                    else if ((ball.Position.Y - ball.Radius) >= _table.SizeY)
                    {
                        ball.Position = new Vector2(ball.Position.X, _table.SizeY - ball.Radius);
                        ball.Velocity = Vector2.Zero;

                    }
            }
    }
}
