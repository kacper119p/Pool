using Data;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Logic;

public class PoolCollisionSolver : ICollisionSolver
{
    private readonly ITable _table;
    private readonly float _interval;
    private readonly Timer _timer;
    private readonly LinkedList<IBall> _locked;

    public PoolCollisionSolver(ITable table, float interval)
    {
        _table = table;
        _interval = interval;
        _locked = new LinkedList<IBall>();
        _timer = new Timer(interval);
        _timer.AutoReset = true;
        _timer.Elapsed += Tick;
        _timer.Start();
    }

    private void Tick(Object? source, ElapsedEventArgs e)
    {
        lock (_table.Lock)
        {
            for (int i = 0; i < _table.BallCount; i++)
            {
                Monitor.Enter(_table.Balls[i].Lock);
                _locked.AddLast(_table.Balls[i]);
            }

            Task.Delay(1);

            foreach(IBall ball in _locked)
            {
                Monitor.Exit(ball.Lock);
            }
            _locked.Clear();
        }
    }



    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
