using System.Windows.Input;

namespace Client.Domain.Services;

/// <summary>
/// Represents a command that can be executed, with optional logic to determine if it can be executed.
/// Implements the <see cref="ICommand"/> interface.
/// </summary>
public class RelayCommand : ICommand
{
    private Action<object> _execute;
    private Predicate<object> _canExecute = (object parameter) => true;

    /// <summary>
    /// Occurs when the <see cref="ICommand"/> needs to re-evaluate whether it can execute.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class with the specified execute and canExecute actions.
    /// </summary>
    /// <param name="execute">The action to execute when the command is triggered.</param>
    /// <param name="canExecute">The predicate to determine whether the command can be executed. 
    /// Defaults to a predicate that always returns <c>true</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="execute"/> action is null.</exception>
    public RelayCommand(Action<object> execute, Predicate<object> canExecute = default!)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute ?? _canExecute;
    }

    /// <summary>
    /// Determines whether the command can execute given the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter that can be used in the execution logic. It is passed to the <paramref name="canExecute"/> predicate.</param>
    /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
    public bool CanExecute(object parameter) => _canExecute(parameter);

    /// <summary>
    /// Executes the command with the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter that will be passed to the execute logic.</param>
    public void Execute(object parameter) => _execute(parameter);
}

