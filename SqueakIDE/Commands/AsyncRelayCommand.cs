using System;
using System.Threading.Tasks;
using System.Windows.Input;

public class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<Task> execute)
    {
        _execute = execute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter) => !_isExecuting;

    public async void Execute(object parameter)
    {
        if (_isExecuting) return;
        _isExecuting = true;
        try
        {
            await _execute();
        }
        finally
        {
            _isExecuting = false;
        }
    }
} 