namespace Data;

public class Table
{
    private float _sizeX;
    private float _sizeY;
    private List<Ball> _balls;

    public Table(float sizeX, float sizeY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _balls = new List<Ball>();
    }

    public float SizeX => _sizeX;

    public float SizeY => _sizeY;

    public List<Ball> Balls => _balls;

    public void AddBall(Ball ball)
    {
        Balls.Add(ball);
    }
}