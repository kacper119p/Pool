using Data;

namespace Logic;

public class PoolBallsBehaviourFactory : IBallsBehaviourFactory
{
    public IPoolBallsBehaviour Create(IBall ball, float interval)
    {
        return new PoolBallsBehaviour(ball, interval);
    }
}
