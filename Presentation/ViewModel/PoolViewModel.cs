using Presentation.ViewModel.Commands;
using System.Windows.Input;

namespace Presentation.ViewModel;

internal class PoolViewModel
{
    private PoolModel _poolModel;
    private CreateBallsCommand _createBallsCommand = new CreateBallsCommand();

    public ICommand CreateBallsCommand => _createBallsCommand;

    public string SpawnAmountText
    {
        get => _poolModel.SpawnAmountString;
        set
        {
            if (value.Length == 0) { return; }
            if (!uint.TryParse(value, out uint _)) { return; }
            _poolModel.SpawnAmountString = value;
        }
    }

    public PoolViewModel()
    {
        _poolModel = new PoolModel();
    }
}
