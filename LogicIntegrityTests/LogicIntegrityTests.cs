using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Data;
using Logic;
namespace LogicIntegrityTests;
using NUnit.Framework;

public class Tests
{
        private Collection<IBall> _testballs;
        private readonly object _ballsLock = new object();
        [Test]
        public void ControllerTest(){
            _testballs = new ObservableCollection<IBall>();
            Task.Run(async () =>
            {
                ISimulationController controller =  new PoolController(new PoolTable(256,256), new PoolBallsBehaviourFactory(), new PoolCollisionSolverFactory());
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
                ISimulationController controller =  new PoolController(new PoolTable(256,256), new PoolBallsBehaviourFactory(), new PoolCollisionSolverFactory());
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
                controller.OnBallsUpdate -= Controllerhelp;
                controller.Dispose();

            }).GetAwaiter().GetResult();
            
            Task.Run(async () =>
            {
                ISimulationController controller =  new PoolController(new PoolTable(256,256), new PoolBallsBehaviourFactory(), new PoolCollisionSolverFactory());
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
                            IBall ball = new PoolBall(Color.Aqua, Vector2.Zero, Vector2.Zero,1,1);
                            _testballs.Add(ball);
                            _testballs[i].Color = balls[i].Color;
                            _testballs[i].Position = balls[i].Position;
                            _testballs[i].Radius = balls[i].Radius;
                            
                        }

                    }
                }

            }
        }
