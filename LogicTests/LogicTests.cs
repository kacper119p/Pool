using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Data;
using Logic;

namespace LogicTests
{
    public class LogicTests
    {
        [Test]
        public void PoolBallsBehaviourTest()
        {
            IPoolBallsBehaviour behaviour = new SimplifiedPoolBallsBehaviour();
            ITable table = new PoolTable(100,100);
            IBall ball = new PoolBall(Color.Blue, new Vector2(15,15),new Vector2(1,0),1,1 );
            table.AddBall(ball);
            behaviour.Tick(1,table);
            Vector2 support = new Vector2(16, 15);
            Assert.AreEqual(support,table.Balls[0].Position);
        }

        [Test]
        public void ControllerTest()
        {
            IPoolBallsBehaviour behaviour = new SimplifiedPoolBallsBehaviour();
            ISimulationController controller = new PoolController(100,100,behaviour);
            IBall ball = new PoolBall(Color.Blue, new Vector2(15,15),new Vector2(1,0),1,1 );
            Assert.False(controller.);
        }
    }
}