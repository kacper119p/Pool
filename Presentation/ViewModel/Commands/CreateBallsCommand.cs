using Presentation.Model;
using System.Windows.Input;

namespace Presentation.ViewModel.Commands;

internal class CreateBallsCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private PoolModel _model;

    public CreateBallsCommand (PoolModel model)
    {
        _model = model;
    }

    public bool CanExecute(object? parameter)
    {
        return parameter is string;
    }

    public void Execute(object? parameter)
    {
        string? value = (string?)parameter;
        if(int.TryParse(value, out int num))
        {
            _model.CreateBalls(num);
        }
    }
}
