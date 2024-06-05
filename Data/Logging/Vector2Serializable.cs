using System.Numerics;

namespace Data.Logging
{
    [Serializable]
    public struct Vector2Serializable
    {
        public float X => _x;
        public float Y => _y;

        private readonly float _x;
        private readonly float _y;

        public Vector2Serializable(float x, float y) : this()
        {
            _x = x;
            _y = y;
        }

        public static implicit operator Vector2Serializable(Vector2 vector) => new Vector2Serializable(vector.X, vector.Y);
    }
}
