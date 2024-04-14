using System.Drawing;
using System.Numerics;
using Data;

namespace DataTests
{
    public class TableTests
    {

        [Test]
        public void PoolBallTest()
        {
            PoolBall ball = new PoolBall(Color.Blue, Vector2.Zero, Vector2.Zero, 1,1);
            Assert.AreEqual(Color.Blue,ball.Color );
            Assert.AreEqual(Vector2.Zero,ball.Position);
            ball.Position = Vector2.One;
            Assert.AreEqual(Vector2.One,ball.Position);
            Assert.AreEqual(Vector2.Zero,ball.Velocity);
            ball.Velocity = Vector2.One;
            Assert.AreEqual(Vector2.One,ball.Velocity);
            Assert.AreEqual(1,ball.Mass);
            Assert.AreEqual(1,ball.Radius);
        }

        [Test]
        public void IBallTest()
        {
            IBall ball = new PoolBall(Color.Blue, Vector2.Zero, Vector2.Zero, 1,1);
            Assert.AreEqual(Color.Blue,ball.Color );
            Assert.AreEqual(Vector2.Zero,ball.Position);
            ball.Position = Vector2.One;
            Assert.AreEqual(Vector2.One,ball.Position);
            Assert.AreEqual(Vector2.Zero,ball.Velocity);
            ball.Velocity = Vector2.One;
            Assert.AreEqual(Vector2.One,ball.Velocity);
            Assert.AreEqual(1,ball.Mass);
            Assert.AreEqual(1,ball.Radius);
            
        }

        [Test]
        public void PoolTableTest()
        {
            
        }
    }
}