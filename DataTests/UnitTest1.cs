using System.Drawing;
using System.Numerics;
using Data;

namespace DataTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ListTest()
        {
            Ball ball = new Ball(Color.Blue, Vector2.Zero, Vector2.Zero, 1,1);
            Table table = new Table(100,100);
            table.AddBall(ball);
            Assert.True(table.Balls.Contains(ball));
        }
    }
}