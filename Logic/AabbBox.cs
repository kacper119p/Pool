using System.Numerics;

namespace Logic;

public readonly struct AabbBox(Vector2 lowerBound, Vector2 upperBound)
{
    public readonly Vector2 lowerBound = lowerBound;
    public readonly Vector2 upperBound = upperBound;

    public float Area => (upperBound.X - lowerBound.X) * (upperBound.Y - lowerBound.Y);

    public static AabbBox Combine(AabbBox a, AabbBox b)
    {
        Vector2 lowerBound = new Vector2(Math.Min(a.lowerBound.X, b.lowerBound.X),
            Math.Min(a.lowerBound.Y, b.lowerBound.Y));
        Vector2 upperBound = new Vector2(Math.Max(a.upperBound.X, b.upperBound.X),
            Math.Max(a.upperBound.Y, b.upperBound.Y));
        return new AabbBox(lowerBound, upperBound);
    }

    public bool Intersects(AabbBox other) =>
        (lowerBound.X <= other.upperBound.X && upperBound.X >= other.lowerBound.X)
        && (lowerBound.Y <= other.upperBound.Y && upperBound.Y >= other.lowerBound.Y);

    public static bool Intersect(AabbBox a, AabbBox b)
        => a.Intersects(b);

    public static float VolumeDifference(AabbBox a, AabbBox b) => Math.Abs(a.Area - b.Area);
}
