using System.Drawing;
using System.Numerics;

namespace Data;

public class PoolBall : IBall
{
    private readonly object _lock;
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
        _lock = new object();
    }

    public object Lock => _lock;

    public Color Color
    {
        get
        {
            lock (_lock)
            {
                return _color;
            }
        }
        set
        {
            lock (_lock)
            {
                _color = value;
            }
        }
    }

    public Vector2 Position
    {
        get
        {
            lock (_lock)
            {
                return _position;
            }
        }
        set
        {
            lock (_lock)
            {
                _position = value;
            }
        }
    }

    public Vector2 Velocity
    {
        get
        {
            lock (_lock)
            {
                return _velocity;
            }
        }
        set
        {
            lock (_lock)
            {
                _velocity = value;
            }
        }
    }

    public float Mass
    {
        get
        {
            lock (_lock)
            {
                return _mass;
            }
        }
        set
        {
            lock (_lock)
            {
                _mass = value;
            }
        }
    }

    public float Radius
    {
        get
        {
            lock (_lock)
            {
                return _radius;
            }
        }
        set
        {
            lock (_lock)
            {
                _radius = value;
            }
        }
    }
}
