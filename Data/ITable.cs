using System.Collections.ObjectModel;

namespace Data;

public interface ITable
{
    public float SizeX { get; }

    public float SizeY { get; }

    public ReadOnlyCollection<PoolBall> Balls { get; }
    public void AddBall(PoolBall ball);
    public void ClearBalls();
}