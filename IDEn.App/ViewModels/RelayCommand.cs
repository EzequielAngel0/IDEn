using System;
using System.Windows.Input;

namespace IDEn.App.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _exec;
        private readonly Func<object, bool> _can;

        public RelayCommand(Action execute, Func<bool> can = null)
        {
            _exec = _ => execute();
            _can = can == null ? null : (_ => can());
        }

        public RelayCommand(Action<object> execute, Func<object, bool> can = null)
        { _exec = execute; _can = can; }

        public bool CanExecute(object parameter) => _can?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => _exec(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
