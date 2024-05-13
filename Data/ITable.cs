using System.Collections.ObjectModel;

namespace Data;

public interface ITable
{
    public ReaderWriterLock Lock { get; }
    public float SizeX { get; }

    public float SizeY { get; }

    public int BallCount { get; }

    public ReadOnlyCollection<IBall> Balls { get; }
    public void AddBall(IBall ball);
    public void ClearBalls();
    public void RemoveAt(int i);
}
