using Presentation.Model;
using System.Windows.Input;

namespace Presentation.ViewModel.Commands;

internal class CreateBallsCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private PoolModel _model;

    public CreateBallsCommand(PoolModel model)
    {
        _model = model;
    }

    public bool CanExecute(object? parameter)
    {
        return parameter is string;
    }

    public void Execute(object? parameter)
    {
        string? value = parameter as string;
        int num;
        if (value?.Length == 0) { num = 0; }
        else if (int.TryParse(value, out num)) { }
        else { return; }
        _model.CreateBalls(num);
    }
}
