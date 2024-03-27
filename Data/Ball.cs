using System.Drawing;
using System.Numerics;

namespace Data;

public class Ball
{
    private readonly Color _color;
    private Vector2 _position;
    private Vector2 _forward;
    private readonly float _mass;
    private readonly float _radius;

    public Ball(Color color, Vector2 position, Vector2 forward, float mass, float radius)
    {
        _color = color;
        _position = position;
        _forward = forward;
        _mass = mass;
        _radius = radius;
    }

    public Color Color => _color;

    public Vector2 Position => _position;

    public Vector2 Forward => _forward;

    public float Mass => _mass;

    public float Radius => _radius;
}