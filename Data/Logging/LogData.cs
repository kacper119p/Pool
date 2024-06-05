using System.Numerics;

namespace Data.Logging;

[Serializable]
public readonly struct LogData
{
    public DateTime Time => _time;
    public double TimeBetweenTicksMs => _timeBetweenTicksMs;
    public int BallId => _ballId;
    public int CollisionsThisTick => _collisionsThisTick;
    public int PotentialCollisionsThisTick => _potentialCollisionsThisTick;
    public List<int> Collisions => _collisions;

    public Vector2Serializable Location => _location;

    private readonly DateTime _time;
    private readonly double _timeBetweenTicksMs;
    private readonly int _ballId;
    private readonly int _collisionsThisTick;
    private readonly int _potentialCollisionsThisTick;
    private readonly Vector2Serializable _location;
    private readonly List<int> _collisions;

    public LogData(DateTime time, double timeBetweenTicksMs, int ballId, int collisionsThisTick,
        int potentialCollisionsThisTick, Vector2Serializable location, List<int> collisions)
    {
        this._time = time;
        this._timeBetweenTicksMs = timeBetweenTicksMs;
        this._ballId = ballId;
        this._collisionsThisTick = collisionsThisTick;
        _potentialCollisionsThisTick = potentialCollisionsThisTick;
        _location = location;
        _collisions = collisions;
    }
}
