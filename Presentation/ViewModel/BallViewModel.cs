using Data;
using System.Drawing;
using System.Numerics;

namespace Presentation.ViewModel
{
    class BallViewModel
    {
        private readonly IBall _ball;

        public Color Color => _ball.Color;

        public float Radius => _ball.Radius;

        public float PositionX
        {
            get => _ball.Position.X;
        }

        public float PositionY
        {
            get => _ball.Position.Y;
        }

        public BallViewModel(IBall ball)
        {
            _ball = ball;
        }
    }
}
