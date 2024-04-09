using System.Diagnostics;
using System.Windows.Input;

namespace Presentation.Commands;

internal class CreateBallsCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        Debug.WriteLine("a");
    }
}
