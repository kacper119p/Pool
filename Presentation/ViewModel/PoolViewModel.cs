using Data;
using Presentation.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Input;

namespace Presentation.ViewModel;

internal class PoolViewModel
{
    private readonly PoolModel _poolModel;
    private readonly CreateBallsCommand _createBallsCommand = new CreateBallsCommand();

    public ICommand CreateBallsCommand => _createBallsCommand;
    private readonly ObservableCollection<BallViewModel> _balls;
    public ObservableCollection<BallViewModel> Balls => _balls;

    public string SpawnAmountText
    {
        get => _poolModel.SpawnAmountString;
        set
        {
            if (!uint.TryParse(value, out uint _) && value.Length != 0)
            {
                return;
            }

            if (value.Length > 3)
            {
                return;
            }

            _poolModel.SpawnAmountString = value;
        }
    }

    public PoolViewModel()
    {
        _poolModel = new PoolModel();
        _balls = new ObservableCollection<BallViewModel>{
            new BallViewModel(new PoolBall(Color.CornflowerBlue, new Vector2(50, 50), Vector2.Zero, 1, 10)),
            new BallViewModel(new PoolBall(Color.LawnGreen, new Vector2(100, 50), Vector2.Zero, 1, 25)),
            new BallViewModel(new PoolBall(Color.Crimson, new Vector2(500, 300), Vector2.Zero, 1, 10)),
        };
    }
}
