using System.Numerics;
using Data;

namespace Logic;

public class SimplifiedPoolBallsBehaviour : IPoolBallsBehaviour
{
    public void Tick(float deltaTime, ITable table)
    {
        foreach (IBall ball in table.Balls)
        {
            ball.Position += ball.Velocity * deltaTime;
            if ((ball.Position.X - ball.Radius) < 0)
            {
                ball.Position = new Vector2(ball.Radius, ball.Position.Y);
                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
            }
            else if ((ball.Position.X + ball.Radius) > table.SizeX)
            {
                ball.Position = new Vector2(table.SizeX - ball.Radius, ball.Position.Y);
                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);

            }

            if ((ball.Position.Y - ball.Radius) < 0)
            {
                ball.Position = new Vector2(ball.Position.X, ball.Radius);
                ball.Velocity = new Vector2(ball.Velocity.X,- ball.Velocity.Y);
            }
            else if ((ball.Position.Y + ball.Radius) > table.SizeY)
            {
                ball.Position = new Vector2(ball.Position.X, table.SizeY - ball.Radius);
                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);

            }
        }
    }
}
