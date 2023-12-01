using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeManager.WPF.Commands
{
    // ICommand implementation that allows binding to methods.
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;    // Action to execute when the command is triggered.
        private readonly Func<object, bool> _canExecute; // Function to determine if the command can be executed.

        // Constructor for the RelayCommand.
        // The 'execute' parameter is the action to be executed when the command is triggered.
        // The 'canExecute' parameter is an optional function to determine if the command can be executed.
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Event that is raised when the ability to execute the command changes.
        public event EventHandler CanExecuteChanged
        {
            // Wire up the CanExecuteChanged event to the CommandManager's RequerySuggested event.
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Determines if the command can be executed.
        // If a 'canExecute' function is provided, it is used to make the determination.
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // Executes the command, invoking the associated action.
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
