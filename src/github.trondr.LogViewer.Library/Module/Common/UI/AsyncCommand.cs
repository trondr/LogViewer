using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace github.trondr.LogViewer.Library.Module.Common.UI
{
    /// <summary>
    /// Source: http://enumeratethis.com/2012/06/14/asynchronous-commands-in-metro-wpf-silverlight/
    /// </summary>
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public AsyncCommand(Func<Task> execute) : this(execute, () => true) { }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            // if the command is not executing, execute the users' can execute logic
            return !_isExecuting && _canExecute();
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            // tell the button that we're now executing...
            _isExecuting = true;
            OnCanExecuteChanged();
            try
            {
                // execute user code
                await _execute();
            }
            finally
            {
                // tell the button we're done
                _isExecuting = false;
                OnCanExecuteChanged();
            }
        }

        protected virtual void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null) CanExecuteChanged(this, new EventArgs());
        }
    }
}