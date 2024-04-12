using Data;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;

namespace Logic;

public interface ISimulationController : IDisposable
{
    public event EventHandler<ReadOnlyCollection<IBallData>>? OnBallsUpdate;

    public object Lock { get; }

    public void AddBall(Color color, Vector2 position, Vector2 velocity, float mass, float radius);
    public void RemoveBalls();
}
