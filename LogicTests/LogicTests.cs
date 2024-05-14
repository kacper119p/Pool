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

        public object Lock { get; }

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
        private readonly ReaderWriterLock _lock = new ReaderWriterLock();
        private readonly float _sizeX;
        private readonly float _sizeY;
        private readonly List<IBall> _balls;

        public TestTable()
        {
            _sizeX = 256;
            _sizeY = 256;
            _balls = new List<IBall>();

        }

        public ReaderWriterLock Lock => _lock;
        public float SizeX => _sizeX;

        public float SizeY => _sizeY;

        public int BallCount
        {
            get
            {
                _lock.AcquireReaderLock(60000);
                int result = _balls.Count;
                _lock.ReleaseReaderLock();
                return result;
            }
        }

        public ReadOnlyCollection<IBall> Balls
        {
            get
            {
                _lock.AcquireReaderLock(60000);
                ReadOnlyCollection<IBall> result = _balls.AsReadOnly();
                _lock.ReleaseReaderLock();
                return result;
            }
        }

        public void AddBall(IBall ball)
        {
            _lock.AcquireWriterLock(60000);
            _balls.Add(ball);
            _lock.ReleaseWriterLock();
        }

        public void ClearBalls()
        {
            _lock.AcquireWriterLock(60000);
            _balls.Clear();
            _lock.ReleaseWriterLock();
        }

        public void RemoveAt(int i)
        {
            _lock.AcquireWriterLock(60000);
            _balls.RemoveAt(i);
            _lock.ReleaseWriterLock();
        }
    }

    public class LogicTests
    {
        private Collection<IBall> _testballs;
        private readonly object _ballsLock = new object();
        [Test]
        public void ControllerTest(){
            Task.Run(async () =>
            {
                _testballs = new ObservableCollection<IBall>();
                ISimulationController controller =  new PoolController(new TestTable(), new PoolBallsBehaviourFactory(), new PoolCollisionSolverFactory());
                controller.OnBallsUpdate += Controllerhelp;
                Color color = Color.Blue;
                Vector2 position = new Vector2(255, 0);
                Vector2 velocity = new Vector2(-1, 1);
                float radius = 1;
                float mass = radius * radius;
                    lock (controller.Lock)
                    {
                        controller.AddBall(color, position, velocity, mass, radius);
                    }
                    await Task.Run(() => WaitForUpdate(1));
                    
                    lock (controller.Lock)
                    {
                        controller.AddBall(Color.Red, new Vector2(0,255), new Vector2(1,-1), mass, radius);
                    }

                await Task.Run(() => WaitForUpdate(2));
                await Task.Run(() => WaitChange());
                 lock (_ballsLock)
                 {
                     Assert.AreEqual(2,_testballs.Count);
                     Assert.AreNotEqual(_testballs[0].Color, _testballs[1].Color);
                     Assert.IsTrue(_testballs[0].Position.X<255);
                     Assert.IsTrue(_testballs[0].Position.Y>0);
                     Assert.IsTrue(_testballs[1].Position.X>0);
                     Assert.IsTrue(_testballs[1].Position.Y<255);
                 }
                controller.RemoveBalls();
                await Task.Run(() => WaitZero());
                lock (_ballsLock)
                {
                    Assert.IsEmpty(_testballs);
                }

            }).GetAwaiter().GetResult();
        }

        [Test]
        public void CollisionTests()
        {
            Task.Run(async () =>
            {
                _testballs = new ObservableCollection<IBall>();
                ISimulationController controller =  new PoolController(new TestTable(), new PoolBallsBehaviourFactory(), new PoolCollisionSolverFactory());
                controller.OnBallsUpdate += Controllerhelp;
                Color color = Color.Blue;
                Vector2 position = new Vector2(10, 10);
                Vector2 velocity = new Vector2(0, 1);
                float radius = 1;
                float mass = radius * radius;
                lock (controller.Lock)
                {
                    controller.AddBall(color, position, velocity, mass, radius);
                }
                await Task.Run(() => WaitForUpdate(1));
                    
                lock (controller.Lock)
                {
                    controller.AddBall(Color.Red, new Vector2(10,11), new Vector2(0,-1), mass, radius);
                }

                await Task.Run(() => WaitForUpdate(2));
                await Task.Run(() => WaitChange());
                lock (_ballsLock)
                {
                    Assert.AreEqual(2,_testballs.Count);
                    Assert.AreNotEqual(_testballs[0].Color, _testballs[1].Color);
                    Assert.IsTrue(_testballs[0].Position.Y<11);
                    Assert.IsTrue(_testballs[1].Position.Y>11);
                }
                controller.RemoveBalls();
                await Task.Run(() => WaitZero());
                lock (_ballsLock)
                {
                    Assert.IsEmpty(_testballs);
                }

            }).GetAwaiter().GetResult();
            
        }

        public async Task WaitForUpdate(int amount)
        {
            while (_testballs.Count <amount)
            {
                
            }
        }
        public async Task WaitChange()
        {
            Collection<IBall> _testballs2 = _testballs;
            while (_testballs2[0].Position.Y == _testballs[0].Position.Y && _testballs2[0].Position.X == _testballs[0].Position.X)
            {
                
            }
        }

        public async Task WaitZero()
        {
            while (_testballs.Count !=0)
            {
                
            }
        }

            public void Controllerhelp(object? sender, ReadOnlyCollection<IBallData> balls)
            {
                lock (_ballsLock)
                {
                    if (balls.Count ==0 )
                    {
                        _testballs.Clear();
                    }
                    for (int i = 0; i < balls.Count; i++)
                    {
                        try
                        {
                            _testballs[i].Color = balls[i].Color;
                            _testballs[i].Position = balls[i].Position;
                            _testballs[i].Radius = balls[i].Radius;
                        }
                        catch
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
    }
