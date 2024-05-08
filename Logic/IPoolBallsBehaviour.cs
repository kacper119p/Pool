using Data;

namespace Logic;

public interface IPoolBallsBehaviour : IDisposable
{
    public void Tick(float deltaTime, ITable table);
}
