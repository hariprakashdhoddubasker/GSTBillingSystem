using System;
using System.Windows.Input;

namespace RishiSilvers.Helpers
{
    public class Command : ICommand
    {
        private readonly Action<object> myExecuteMethod;
        private readonly Func<object, bool> myCanExecuteMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        /// <param name="executeMethod">Logic to execute</param>
        /// <param name="canExecuteMethod">Condition to check for before execution</param>
        public Command(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.myExecuteMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod), @"You must specify an Action<T>.");
            this.myCanExecuteMethod = canExecuteMethod;
        }

        /// <inheritdoc />
        /// <summary>
        /// Event to check if Can Execute Changed
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc />
        /// <summary>
        /// Condition to check for before execution
        /// </summary>
        /// <param name="parameter">Object Parameter</param>
        /// <returns>Condition to check</returns>
        public bool CanExecute(object parameter)
        {
            return this.myCanExecuteMethod(parameter);
        }

        /// <inheritdoc />
        /// <summary>
        /// Logic to execute
        /// </summary>
        /// <param name="parameter">Object Parameter</param>
        public void Execute(object parameter)
        {
            this.myExecuteMethod(parameter);
        }

        /// <summary>
        /// Raise  Can Execute Changed event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }

    public class RelayCommand<T> : ICommand
    {
        Action<T> _TargetExecuteMethod;
        Func<T, bool> _TargetCanExecuteMethod;

        public RelayCommand(Action<T> executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                T tparm = (T)parameter;
                return _TargetCanExecuteMethod(tparm);
            }
            if (_TargetExecuteMethod != null)
            {
                return true;
            }
            return false;
        }

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
        // Prism commands solve this in their implementation
        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod((T)parameter);
            }
        }
        #endregion
    }
}
