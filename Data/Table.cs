namespace Data;

public class Table
{
    private float _sizeX;
    private float _sizeY;
    private List<Ball> _balls;

    public Table(float sizeX, float sizeY, List<Ball> balls)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _balls = balls;
    }

    public float SizeX => _sizeX;

    public float SizeY => _sizeY;

    public List<Ball> Balls => _balls;
}