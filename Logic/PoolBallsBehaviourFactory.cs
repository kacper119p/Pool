using Data;

namespace Logic;

public class PoolBallsBehaviourFactory : IBallsBehaviourFactory
{
    public IPoolBallsBehaviour Create(IBall ball, float interval, AabbTree collisionTree, int id, ITable table)
    {
        return new PoolBallsBehaviour(ball, interval, collisionTree, id, table);
    }
}
