using System.Drawing;
using System.Numerics;

namespace Logic;

public interface IBallData
{
    public Color Color { get; }
    public Vector2 Position { get; }
    public float Radius { get; }
}
