using System.Drawing;
using System.Numerics;

namespace Data;

public interface IBall
{
    public Color Color { get; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Mass { get; }
    public float Radius { get; }
    public float positionX { get; }
    public float positionY { get; }
}
