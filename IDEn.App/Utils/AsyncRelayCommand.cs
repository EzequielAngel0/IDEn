using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IDEn.App.Utils
{
    /// <summary>
    /// Comando asíncrono reutilizable en toda la app.
    /// </summary>
    public sealed class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _exec;
        private readonly Func<bool> _can;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _exec = execute ?? throw new ArgumentNullException(nameof(execute));
            _can = canExecute;
        }

        public bool CanExecute(object parameter) => _can?.Invoke() ?? true;
        public async void Execute(object parameter) => await _exec();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
