using System.Drawing;
using System.Numerics;

namespace Data;

public interface IBall
{
    public Color Color { get; }
    public Vector2 Position { get; }
    public Vector2 Velocity { get; }
    public float Mass { get; }
    public float Radius { get; }
}
