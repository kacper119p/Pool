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
            PoolTable table = new PoolTable(256, 256);
            Assert.AreEqual(256, table.SizeX);
            Assert.AreEqual(256, table.SizeY);
            IBall ball = new PoolBall(Color.Blue, Vector2.Zero, Vector2.Zero, 1,1);
            Assert.False(table.Balls.Contains(ball));
            table.AddBall(ball);
            Assert.True(table.Balls.Contains(ball));
            table.ClearBalls();
            Assert.False(table.Balls.Contains(ball));
            
        }
        
        [Test]
        public void ITableTest()
        {
            ITable table = new PoolTable(256, 256);
            Assert.AreEqual(256, table.SizeX);
            Assert.AreEqual(256, table.SizeY);
            IBall ball = new PoolBall(Color.Blue, Vector2.Zero, Vector2.Zero, 1,1);
            Assert.False(table.Balls.Contains(ball));
            table.AddBall(ball);
            Assert.True(table.Balls.Contains(ball));
            table.ClearBalls();
            Assert.False(table.Balls.Contains(ball));
            
        }
    }
}