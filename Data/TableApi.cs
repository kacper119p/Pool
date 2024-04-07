using System.Collections.ObjectModel;

namespace Data;

public abstract class TableApi
{

    public abstract void AddBall(Ball ball);
    public abstract void ClearBalls();
    public abstract ReadOnlyCollection<Ball> GetBalls();
}