using Data;

namespace Logic;

public class PoolBallsBehaviourFactory : IBallsBehaviourFactory
{
    public IPoolBallsBehaviour Create(IBall ball, ITable table, float interval)
    {
        return new PoolBallsBehaviour(ball, table, interval);
    }
}
