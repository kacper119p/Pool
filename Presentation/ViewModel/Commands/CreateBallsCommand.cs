using System.Diagnostics;
using System.Windows.Input;

namespace Presentation.ViewModel.Commands;

internal class CreateBallsCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (parameter is not string value)
        {
            return;
        }

        Debug.WriteLine(value);
    }
}
