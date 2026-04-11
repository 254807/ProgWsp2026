using System;
using System.Windows.Input;

namespace ConcurrentProgramming.ViewModel;

/// <summary>
/// A <see cref="ICommand"/> implemented via delegates passed through the constructor.
/// </summary>
/// <param name="execute">The delegate to run opon executing the command.</param>
/// <param name="canExecute">The delegate to run to check whether the command may execute.</param>
public sealed class DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) : ICommand
{
    /// <summary>
    /// Cans the execute.
    /// </summary>
    /// <param name="parameter">The parameter</param>
    /// <returns>True if can, false otherwise</returns>
    public bool CanExecute(object? parameter)
    {
        return canExecute?.Invoke(parameter) ?? true;
    }

    /// <summary>
    /// Executes the parameter
    /// </summary>
    /// <param name="parameter">The parameter</param>
    public void Execute(object? parameter)
    {
        execute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
