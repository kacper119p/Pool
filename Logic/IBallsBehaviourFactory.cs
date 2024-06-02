using Data;
using Data.Logging;

namespace Logic;

public interface IBallsBehaviourFactory
{
    public IPoolBallsBehaviour
        Create(IBall ball, float interval, AabbTree collisionTree, int id, ITable table, ILogger logger);
}
