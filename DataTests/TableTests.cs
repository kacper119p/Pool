using System.Drawing;
using System.Numerics;
using Data;

namespace DataTests
{
    public class TableTests
    {

        [Test]
        public void ListTest()
        {
            PoolBall ball = new PoolBall(Color.Blue, Vector2.Zero, Vector2.Zero, 1,1);
            PoolTable table = new PoolTable(100,100);
        }
        
        [Test]
        public void ApiTest()
        {
            PoolBall ball = new PoolBall(Color.Blue, Vector2.Zero, Vector2.Zero, 1,1);
            ITable table = new PoolTable(100,100);
        }
    }
}