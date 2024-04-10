using Data;
using System.Drawing;
using System.Numerics;

namespace Presentation.ViewModel
{
    class BallViewModel
    {
        private IBall _ball;

        public Color Color => _ball.Color;

        public float Radius => _ball.Radius;

        public float positionX
        {
            get => _ball.Position.X;
        }

        public float positionY
        {
            get => _ball.Position.Y;
        }

        public BallViewModel(IBall ball)
        {
            _ball = ball;
        }
    }
}
