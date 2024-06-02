namespace Data.Logging;

[Serializable]
public readonly struct LogData
{
    public DateTime Time => _time;
    public double TimeBetweenTicksMs => _timeBetweenTicksMs;
    public int BallId => _ballId;
    public int CollisionsThisTick => _collisionsThisTick;
    public int PotentialCollisionsThisTick => _potentialCollisionsThisTick;

    private readonly DateTime _time;
    private readonly double _timeBetweenTicksMs;
    private readonly int _ballId;
    private readonly int _collisionsThisTick;
    private readonly int _potentialCollisionsThisTick;

    public LogData(DateTime time, double timeBetweenTicksMs, int ballId, int collisionsThisTick,
        int potentialCollisionsThisTick)
    {
        this._time = time;
        this._timeBetweenTicksMs = timeBetweenTicksMs;
        this._ballId = ballId;
        this._collisionsThisTick = collisionsThisTick;
        _potentialCollisionsThisTick = potentialCollisionsThisTick;
    }
}
