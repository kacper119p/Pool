using System.Collections.ObjectModel;

namespace Data;

public class PoolTable : ITable
{
    private readonly object _lock;
    private readonly float _sizeX;
    private readonly float _sizeY;
    private readonly List<IBall> _balls;

    public PoolTable(float sizeX, float sizeY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _balls = new List<IBall>();
        _lock = new object();
    }

    public object Lock => _lock;
    public float SizeX => _sizeX;

    public float SizeY => _sizeY;

    public int BallCount
    {
        get
        {
            lock (_lock)
            {
                return _balls.Count;
            }
        }
    }

    public ReadOnlyCollection<IBall> Balls => _balls.AsReadOnly();

    public void AddBall(IBall ball)
    {
        lock (_lock)
        {
            _balls.Add(ball);
        }
    }

    public void ClearBalls()
    {
        lock (_lock)
        {
            _balls.Clear();
        }
    }

    public void RemoveAt(int i)
    {
        lock (_lock)
        {
            _balls.RemoveAt(i);
        }
    }
}
