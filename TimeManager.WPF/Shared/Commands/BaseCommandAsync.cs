using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeManager.WPF.Shared.Commands
{

    public abstract class AsyncCommandBase : ICommand
    {
        //only cares about the signature and does not give a fuck about implementation.. Amaizing!
        private readonly Action<Exception> _onException;

        private bool _isExecuting;
        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                OnCanExecuteChanged();
            }
        }
        public event EventHandler CanExecuteChanged;


        public AsyncCommandBase(Action<Exception> onException = null)
        {
            _onException = onException;
        }


        public bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }



    }
}
