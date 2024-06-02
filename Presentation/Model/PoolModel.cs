using Data;
using Logic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using System.Windows;
using System.Windows.Data;
using Data.Logging;

namespace Presentation.Model;

internal class PoolModel
{
    private readonly object _ballsLock;
    private readonly ISimulationController _simulationController;
    private readonly Random _random = new Random();

    private ObservableCollection<BallRenderData> _balls;
    public ObservableCollection<BallRenderData> Balls => _balls;

    public PoolModel()
    {
        _simulationController = new PoolController(new PoolTable(800, 350),
            new PoolBallsBehaviourFactory(),
            new PoolCollisionSolverFactory(),
            new FileLogger("log.json"));

        _balls = new ObservableCollection<BallRenderData>();

        _simulationController.OnBallsUpdate += UpdateBalls;
        _ballsLock = new object();
        BindingOperations.EnableCollectionSynchronization(_balls, _ballsLock);

        Application.Current.Dispatcher.ShutdownStarted += Dispose;
    }

    public void CreateBalls(int amount)
    {
        lock (_simulationController.Lock)
        {
            lock (_ballsLock)
            {
                if (amount < 0)
                {
                    amount = -amount;
                    amount = Math.Min(amount, _balls.Count);
                    _simulationController.RemoveBalls(amount);
                    for (int i = 0; i < amount; i++)
                    {
                        _balls.RemoveAt(_balls.Count - 1);
                    }

                    return;
                }

                for (int i = 0; i < amount; i++)
                {
                    Color color = Color.FromArgb(_random.Next(int.MinValue, int.MaxValue));
                    Vector2 position = new Vector2(_random.Next(100, 700), _random.Next(100, 250));
                    Vector2 velocity = new Vector2(_random.NextSingle() * 500.0f - 250.0f,
                        _random.NextSingle() * 500.0f - 250.0f);
                    float radius = _random.Next(10, 20);
                    float mass = radius * radius;
                    _simulationController.AddBall(color, position, velocity, mass, radius);
                    _balls.Add(new BallRenderData());
                }
            }
        }
    }

    public void ClearBalls()
    {
        lock (_simulationController.Lock)
        {
            lock (_ballsLock)
            {
                _simulationController.RemoveBalls();
                _balls.Clear();
            }
        }
    }

    private void UpdateBalls(object? sender, ReadOnlyCollection<IBallData> balls)
    {
        lock (_ballsLock)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                _balls[i].Center = new(balls[i].Position.X, balls[i].Position.Y);
                _balls[i].Color = balls[i].Color;
                _balls[i].Radius = balls[i].Radius;
            }
        }
    }

    public void Dispose(object? sender, EventArgs args)
    {
        lock (_simulationController.Lock)
        {
            _simulationController.Dispose();
        }
    }
}
