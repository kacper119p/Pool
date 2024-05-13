using System.Collections.ObjectModel;

namespace Data;

public class PoolTable : ITable
{
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    private readonly float _sizeX;
    private readonly float _sizeY;
    private readonly List<IBall> _balls;

    public PoolTable(float sizeX, float sizeY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _balls = new List<IBall>();

    }

    public ReaderWriterLock Lock => _lock;
    public float SizeX => _sizeX;

    public float SizeY => _sizeY;

    public int BallCount
    {
        get
        {
            _lock.AcquireReaderLock(60000);
            int result = _balls.Count;
            _lock.ReleaseReaderLock();
            return result;
        }
    }

    public ReadOnlyCollection<IBall> Balls
    {
        get
        {
            _lock.AcquireReaderLock(60000);
            ReadOnlyCollection<IBall> result = _balls.AsReadOnly();
            _lock.ReleaseReaderLock();
            return result;
        }
    }

    public void AddBall(IBall ball)
    {
        _lock.AcquireWriterLock(60000);
        _balls.Add(ball);
        _lock.ReleaseWriterLock();
    }

    public void ClearBalls()
    {
        _lock.AcquireWriterLock(60000);
        _balls.Clear();
        _lock.ReleaseWriterLock();
    }

    public void RemoveAt(int i)
    {
        _lock.AcquireWriterLock(60000);
        _balls.RemoveAt(i);
        _lock.ReleaseWriterLock();
    }
}
