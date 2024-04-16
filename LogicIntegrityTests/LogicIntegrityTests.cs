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
    public void PoolBallsBehaviourTest()
    {
        IPoolBallsBehaviour behaviour = new SimplifiedPoolBallsBehaviour();
        ITable table = new PoolTable(100, 100);
        IBall ball = new PoolBall(
            Color.Blue,
            new Vector2(15, 15),
            new Vector2(1, 0), 1, 1);
        table.AddBall(ball);
        behaviour.Tick(1, table);
        Vector2 support = new Vector2(16, 15);
        Assert.AreEqual(support, table.Balls[0].Position);
    }

    [Test]
    public void ControllerTest()
    {
        Task.Run(async () =>
        {
            _testballs = new ObservableCollection<IBall>();
            IPoolBallsBehaviour behaviour = new SimplifiedPoolBallsBehaviour();
            using ISimulationController controller = new PoolController(new PoolTable(100, 100), behaviour);
            controller.OnBallsUpdate += Controllerhelp;
            controller.AddBall(
                Color.Blue,
                new Vector2(19, 15),
                new Vector2(10, -10),
                1,
                1);
            controller.AddBall(
                Color.Red,
                new Vector2(19, 15),
                new Vector2(-10, 10),
                1,
                1);
            await Task.Run(() => WaitForUpdate());
            lock (_ballsLock)
            {
                Assert.AreEqual(2, _testballs.Count);
                Assert.AreNotEqual(_testballs[0].Color, _testballs[1].Color);
                Assert.IsTrue(_testballs[0].Position.X > 19);
                Assert.IsTrue(_testballs[0].Position.Y < 15);
                Assert.IsTrue(_testballs[1].Position.Y < 19);
                Assert.IsTrue(_testballs[1].Position.Y > 15);
            }

            controller.RemoveBalls();
            await Task.Run(() => WaitZero());
            lock (_ballsLock)
            {
                Assert.IsEmpty(_testballs);
            }


        }).GetAwaiter().GetResult();
    }

    public async Task WaitForUpdate()
    {
        while (_testballs.Count < 2)
        {

        }
    }

    public async Task WaitZero()
    {
        while (_testballs.Count != 0)
        {

        }
    }

    public void Controllerhelp(object? sender, ReadOnlyCollection<IBallData> balls)
    {
        lock (_ballsLock)
        {
            if (balls.Count == 0)
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
                    IBall ball = new PoolBall(Color.Aqua, Vector2.Zero, Vector2.Zero, 1, 1);
                    _testballs.Add(ball);
                    _testballs[i].Color = balls[i].Color;
                    _testballs[i].Position = balls[i].Position;
                    _testballs[i].Radius = balls[i].Radius;

                }

            }
        }

    }
}