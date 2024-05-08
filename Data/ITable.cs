using System.Collections.ObjectModel;

namespace Data;

public interface ITable
{
    public object Lock { get; }
    public float SizeX { get; }

    public float SizeY { get; }

    public ReadOnlyCollection<IBall> Balls { get; }
    public void AddBall(IBall ball);
    public void ClearBalls();
    public void RemoveBalls(int amount);
}
