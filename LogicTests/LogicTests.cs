using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Data;
using Logic;

namespace LogicTests
{
    public class TestBall : IBall
    {
        private Color _color;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _mass;
        private float _radius;

        public TestBall(Color color, Vector2 position, Vector2 velocity)
        {
            _color = color;
            _position = position;
            _velocity = velocity;
            _mass = 1;
            _radius = 1;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public float Mass
        {
            get => _mass;
            set => _mass = value;
        }

        public float Radius
        {
            get => _radius;
            set => _radius = value;
        }
    }
    public class TestTable : ITable
    {
        private readonly float _sizeX;
        private readonly float _sizeY;
        private readonly List<IBall> _balls;

        public TestTable()
        {
            _sizeX = 100;
            _sizeY = 100;
            _balls = new List<IBall>();
            IBall ball = new TestBall(
                Color.Blue, 
                new Vector2(15,15),
                new Vector2(1,0));
            _balls.Add(ball);
        }

        public float SizeX => _sizeX;

        public float SizeY => _sizeY;

        public ReadOnlyCollection<IBall> Balls => _balls.AsReadOnly();

        public void AddBall(IBall ball)
        {
            _balls.Add(ball);
        }

        public void ClearBalls()
        {
            _balls.Clear();
        }

    }
    public class LogicTests
    {
        private Collection<IBall> _testballs;
        [Test]
        public void PoolBallsBehaviourTest()
        {
            IPoolBallsBehaviour behaviour = new SimplifiedPoolBallsBehaviour();
            ITable table = new PoolTable(100,100);
            IBall ball = new TestBall(
                Color.Blue,
                new Vector2(15,15),
                new Vector2(1,0) );
            table.AddBall(ball);
            behaviour.Tick(1,table);
            Vector2 support = new Vector2(16, 15);
            Assert.AreEqual(support,table.Balls[0].Position);
        }

        [Test]
        public void ControllerTest()
        {
            Task.Run(async () =>
            {
                _testballs = new ObservableCollection<IBall>();
                IPoolBallsBehaviour behaviour = new SimplifiedPoolBallsBehaviour();
                ISimulationController controller = new PoolController(new TestTable(), behaviour);
                controller.OnBallsUpdate += Controllerhelp;
                controller.AddBall(
                    Color.Blue,
                    new Vector2(15, 15),
                    new Vector2(1, 0),
                    1,
                    1);
                controller.AddBall(
                    Color.Red,
                    new Vector2(15, 15),
                    new Vector2(1, 0),
                    1,
                    1);
                await Task.Run(() => waitForUpdate());
            }).GetAwaiter().GetResult();
            Console.Write(_testballs.Count);
        }

        public async Task waitForUpdate()
        {
            while (_testballs.Count <2)
            {
                
            }
        }

            public void Controllerhelp(object? sender, ReadOnlyCollection<IBallData> balls)
            {
                for (int i = 0; i<balls.Count; i++)
                {
                    try
                    {
                        _testballs[i].Color = balls[i].Color;
                        _testballs[i].Position = balls[i].Position;
                        _testballs[i].Radius = balls[i].Radius;
                    }
                    catch (Exception e)
                    {
                        IBall ball = new TestBall(Color.Aqua, Vector2.Zero, Vector2.Zero);
                        _testballs.Add(ball);
                        _testballs[i].Color = balls[i].Color;
                        _testballs[i].Position = balls[i].Position;
                        _testballs[i].Radius = balls[i].Radius;
                    }
                }

            }
        }
    }