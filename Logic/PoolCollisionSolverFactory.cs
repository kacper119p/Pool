using Data;

namespace Logic
{
    public class PoolCollisionSolverFactory : ICollisionSolverFactory
    {
        public ICollisionSolver Create(ITable table, float interval)
            => new PoolCollisionSolver(table, interval);
    }
}
