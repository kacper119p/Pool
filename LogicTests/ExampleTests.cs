using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Data;
using Logic;

namespace LogicTests
{
    public class ExampleTests
    {
        [Test]
        public void Test()
        {
            PoolController poolController = new PoolController();
            TableApi table = new Table(100,100);
            poolController.AddBall(new Ball(Color.Black, Vector2.Zero, Vector2.One, 1,1),table);
        }
    }
}