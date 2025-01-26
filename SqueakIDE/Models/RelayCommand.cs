using System;
using System.Windows.Input;


namespace SqueakIDE.Models
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action action, Func<bool> canExecute = null)
        {
            _execute = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged;
    }
}
