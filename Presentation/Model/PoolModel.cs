using Data;
using Logic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Data;

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
        _simulationController = new PoolController(new PoolTable(800, 350), new SimplifiedPoolBallsBehaviour());

        _balls = new ObservableCollection<BallRenderData>();

        _simulationController.OnBallsUpdate += UpdateBalls;
        _ballsLock = new object();
        BindingOperations.EnableCollectionSynchronization(_balls, _ballsLock);
    }

    public void CreateBalls(int amount)
    {
        lock (_simulationController.Lock)
        {
            lock (_ballsLock)
            {
                _simulationController.RemoveBalls();
                _balls.Clear();
                for (int i = 0; i < amount; i++)
                {
                    Color color = Color.FromArgb(_random.Next(int.MinValue, int.MaxValue));
                    Vector2 position = new Vector2(_random.Next(100, 700), _random.Next(100, 250));
                    Vector2 velocity = new Vector2(_random.NextSingle() * 1000.0f - 500.0f, _random.NextSingle() * 1000.0f - 500.0f);
                    float mass = 1;
                    float radius = _random.Next(10, 50);
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
}
