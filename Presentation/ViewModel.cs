using Presentation.Commands;
using System.Windows.Input;

namespace Presentation;

public class ViewModel
{
    private CreateBallsCommand _createBallsCommand = new CreateBallsCommand();

    public ICommand CreateBallsCommmand => _createBallsCommand;
}
