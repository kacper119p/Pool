using Data;

namespace Logic;

public interface ISimulationController : IDisposable
{
    public void AddBall(IBall ball);
    public void RemoveBalls();

}
