using System.Windows.Input;

namespace Client.Domain.Services;

public class RelayCommand : ICommand
{
    private Action<object> _execute;
    private Predicate<object> _canExecute = (object parameter) => true;

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = default!)
    {
        _execute = execute ?? throw new ArgumentNullException("execute");
        _canExecute = canExecute ?? _canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute(parameter);

    public void Execute(object parameter) => _execute(parameter);
}
