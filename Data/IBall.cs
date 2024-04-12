using System.Drawing;
using System.Numerics;

namespace Data;

public interface IBall
{
    public Color Color { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Mass { get; set; }
    public float Radius { get; set; }
}
