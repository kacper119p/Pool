using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Data;
using Logic;

namespace LogicTests
{
    public class LogicTests
    {
        private Collection<IBall> _balls;
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
            _balls = new ObservableCollection<IBall>();
            controller.OnBallsUpdate += Controllerhelp;
            controller.AddBall(Color.Blue, new Vector2(15,15),new Vector2(1,0),1,1 );
            controller.AddBall(Color.Red, new Vector2(15,15),new Vector2(1,0),1,1 );
            Console.Write(_balls);
        }

            public void Controllerhelp(object? sender, ReadOnlyCollection<IBallData> balls)
            {
                for (int i = 0; i < _balls.Count; i++)
                {
                    _balls[i].Position = balls[i].Position;
                    _balls[i].Color = balls[i].Color;
                    _balls[i].Radius = balls[i].Radius;
                }
            
            }
        }
    }