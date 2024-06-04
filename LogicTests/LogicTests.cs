using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Data;
using Data.Logging;
using Logic;
using System.Text.Json;

namespace LogicTests
{
    public class TestLogger : ILogger
    {
        private readonly Queue<LogData> _queue;
        private readonly ManualResetEvent _hasNewItems = new ManualResetEvent(false);
        private readonly ManualResetEvent _terminate;
        private readonly ManualResetEvent _waiting;
        private readonly WaitHandle[] _waitHandle;
        private readonly string _filePath;

        public TestLogger()
        {
            _filePath = "filePath.log";
            _queue = new Queue<LogData>();
            _hasNewItems = new ManualResetEvent(false);
            _terminate = new ManualResetEvent(false);
            _waiting = new ManualResetEvent(false);
            _waitHandle = new WaitHandle[] { _hasNewItems, _terminate };
        
            Thread threadHandle = new Thread(ProcessRequests);
            threadHandle.Start();

       
        }

        public void LogData(LogData data)
        {
            lock (_queue)
            {
                _queue.Enqueue(data);
            }

            _hasNewItems.Set();
        }

        public void Dispose()
        {
            _terminate.Set();
        }

        private void ProcessRequests()
        {
            while (true)
            {
                _waiting.Set();
                int i = WaitHandle.WaitAny(_waitHandle);
                _hasNewItems.Reset();
                _waiting.Reset();

                Queue<LogData> queueCopy;
                lock (_queue)
                {
                    queueCopy = new Queue<LogData>(_queue);
                    _queue.Clear();
                }

                using (StreamWriter writer = new StreamWriter(_filePath, true))
                {
                    foreach (LogData data in queueCopy)
                    {
                        string str = JsonSerializer.Serialize(data);
                        writer.WriteLine(str);
                    }
                }

                if (i == 1)
                {
                    return;
                }
            }
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Info(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
    
    public class TestBall : IBall
    {
        private Color _color;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _mass;
        private float _radius;
        private readonly object _lock;

        public TestBall(Color color, Vector2 position, Vector2 velocity)
        {
            _color = color;
            _position = position;
            _velocity = velocity;
            _mass = 1;
            _radius = 1;
            _lock = new object();
        }

        
        public object Lock => _lock;

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
            _testballs = new ObservableCollection<IBall>();
            Task.Run(async () =>
            {
                ILogger logger = new TestLogger();
                ISimulationController controller =  new PoolController(new TestTable(), new PoolBallsBehaviourFactory(), new PoolCollisionSolverFactory(),logger);
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
                controller.OnBallsUpdate -= Controllerhelp;
                controller.Dispose();
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void CollisionTests()
        {
            _testballs = new ObservableCollection<IBall>();
            Task.Run(async () =>
            {
                ILogger logger = new TestLogger();
                ISimulationController controller =  new PoolController(new TestTable(), new PoolBallsBehaviourFactory(), new PoolCollisionSolverFactory(),logger);
                controller.OnBallsUpdate += Controllerhelp;
                Color color = Color.Blue;
                Vector2 position = new Vector2(10, 10);
                Vector2 velocity = new Vector2(0, 0.5f);
                float radius = 1;
                float mass = radius * radius;
                lock (controller.Lock)
                {
                    controller.AddBall(color, position, velocity, mass, radius);
                }
                await Task.Run(() => WaitForUpdate(1));
                    
                lock (controller.Lock)
                {
                    controller.AddBall(Color.Red, new Vector2(10,11), new Vector2(0,-0.5f), mass, radius);
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
                controller.OnBallsUpdate -= Controllerhelp;
                controller.Dispose();

            }).GetAwaiter().GetResult();
            
            Task.Run(async () =>
            {
                ILogger logger = new TestLogger();
                ISimulationController controller =  new PoolController(new TestTable(), new PoolBallsBehaviourFactory(), new PoolCollisionSolverFactory(),logger);
                controller.OnBallsUpdate += Controllerhelp;
                Color color = Color.Blue;
                Vector2 position = new Vector2(10, 10);
                Vector2 velocity = new Vector2(1, 0);
                float radius = 1;
                float mass = radius * radius;
                lock (controller.Lock)
                {
                    controller.AddBall(color, position, velocity, mass, radius);
                }
                await Task.Run(() => WaitForUpdate(1));
                    
                lock (controller.Lock)
                {
                    controller.AddBall(Color.Red, new Vector2(11,10), new Vector2(-1,0), mass, radius);
                }

                await Task.Run(() => WaitForUpdate(2));
                await Task.Run(() => WaitChange());
                lock (_ballsLock)
                {
                    Assert.AreEqual(2,_testballs.Count);
                    Assert.AreNotEqual(_testballs[0].Color, _testballs[1].Color);
                    Assert.IsTrue(_testballs[0].Position.X<11);
                    Assert.IsTrue(_testballs[1].Position.X>11);
                }
                controller.RemoveBalls();
                await Task.Run(() => WaitZero());
                lock (_ballsLock)
                {
                    Assert.IsEmpty(_testballs);
                }
                controller.OnBallsUpdate -= Controllerhelp;
                controller.Dispose();

            }).GetAwaiter().GetResult();
            
        }
        [Test]
        public void TreeNegTests()
        {
            _testballs = new ObservableCollection<IBall>();
            Task.Run(async () =>
            {
                ICollisionSolverFactory collisionSolverFactory = new PoolCollisionSolverFactory();
                ITable table = new TestTable();
                IBall ball = new TestBall(Color.Blue, new Vector2(90,90),new Vector2(-1,-1));
                IBall ball1 = new TestBall(Color.Red, new Vector2(101,101),new Vector2(1,1));
                table.AddBall(ball);
                table.AddBall(ball1);
                ICollisionSolver collisionSolver = collisionSolverFactory.Create(table,0.1f);
                collisionSolver.Update();
                Vector2 lowerBound = new Vector2(table.Balls[0].Position.X - table.Balls[0].Radius, table.Balls[0].Position.Y - table.Balls[0].Radius);
                Vector2 upperBound = new Vector2(table.Balls[0].Position.X + table.Balls[0].Radius, table.Balls[0].Position.Y + table.Balls[0].Radius);
                AabbBox bounds = new AabbBox(lowerBound, upperBound);
                
                LinkedList<int> candidates = collisionSolver.CollisionTree.Query(bounds);
                Assert.AreNotEqual(2,candidates.Count);

            }).GetAwaiter().GetResult();
        }

        [Test]
        public void TreePositiveTests()
        {
            _testballs = new ObservableCollection<IBall>();
            Task.Run(async () =>
            {
                ICollisionSolverFactory collisionSolverFactory = new PoolCollisionSolverFactory();
                ITable table = new TestTable();
                IBall ball = new TestBall(Color.Blue, new Vector2(100,100),new Vector2(1,1));
                IBall ball1 = new TestBall(Color.Red, new Vector2(101,101),new Vector2(-1,-1));
                table.AddBall(ball);
                table.AddBall(ball1);
                ICollisionSolver collisionSolver = collisionSolverFactory.Create(table,1f);
                collisionSolver.Update();
                Vector2 lowerBound = new Vector2(table.Balls[0].Position.X - table.Balls[0].Radius, table.Balls[0].Position.Y - table.Balls[0].Radius);
                Vector2 upperBound = new Vector2(table.Balls[0].Position.X + table.Balls[0].Radius, table.Balls[0].Position.Y + table.Balls[0].Radius);
                AabbBox bounds = new AabbBox(lowerBound, upperBound);
                
                LinkedList<int> candidates = collisionSolver.CollisionTree.Query(bounds);
                Assert.AreEqual(2,candidates.Count);

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
            Collection<IBall> testballs2 = _testballs;
            while (testballs2[0].Position.Y == _testballs[0].Position.Y &&
                   testballs2[0].Position.X == _testballs[0].Position.X &&
                   testballs2[1].Position.Y == _testballs[1].Position.Y &&
                   testballs2[1].Position.X == _testballs[1].Position.X)
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
