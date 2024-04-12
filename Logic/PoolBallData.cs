using System.Drawing;
using System.Numerics;

namespace Logic
{
    internal class PoolBallData : IBallData
    {
        public Color Color { get; internal set; }
        public Vector2 Position { get; internal set; }
        public float Mass { get; internal set; }
        public float Radius { get; internal set; }
    }
}
