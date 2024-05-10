using Data;

namespace Logic;

public interface IBallsBehaviourFactory
{
    public IPoolBallsBehaviour Create(IBall ball, float interval);
}
