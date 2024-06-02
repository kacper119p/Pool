using Data;
using Data.Logging;

namespace Logic;

public class PoolBallsBehaviourFactory : IBallsBehaviourFactory
{
    public IPoolBallsBehaviour
        Create(IBall ball, float interval, AabbTree collisionTree, int id, ITable table, ILogger logger)
    {
        return new PoolBallsBehaviour(ball, interval, collisionTree, id, table, logger);
    }
}
