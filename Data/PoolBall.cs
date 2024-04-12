using System.Drawing;
using System.Numerics;

namespace Data;

public class PoolBall : IBall
{
    private Color _color;
    private Vector2 _position;
    private Vector2 _velocity;
    private float _mass;
    private float _radius;

    public PoolBall(Color color, Vector2 position, Vector2 velocity, float mass, float radius)
    {
        _color = color;
        _position = position;
        _velocity = velocity;
        _mass = mass;
        _radius = radius;
    }

    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    public float Mass
    {
        get => _mass;
        set => _mass = value;
    }

    public float Radius
    {
        get => _radius;
        set => _radius = value;
    }
}
