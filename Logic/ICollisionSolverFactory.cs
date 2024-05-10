using Data;

namespace Logic;

public interface ICollisionSolverFactory
{
    public ICollisionSolver Create(ITable table, float interval);
}
