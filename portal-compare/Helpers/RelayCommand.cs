using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace portal_compare.Helpers
{
    /// <summary>
    ///     Class RelayCommand.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        ///     The _can execute method
        /// </summary>
        private readonly Func<object, bool> _canExecuteMethod;

        /// <summary>
        ///     The _execute method
        /// </summary>
        private readonly Action<object> _executeMethod;

        /// <summary>
        ///     Initialize a new instance of <see cref="DelegateCommand{T}" />.
        /// </summary>
        /// <param name="executeMethod">The delegate that is executed when <see cref="Execute" /> is called on the command.</param>
        /// <remarks><see cref="CanExecute" /> always returns true.</remarks>
        public RelayCommand(Action<object> executeMethod) : this(executeMethod, (object meth) => { return true; })
        {
        }

        /// <summary>
        ///     Initialize a new instance of <see cref="DelegateCommand{T}" />.
        /// </summary>
        /// <param name="executeMethod">The delegate that is executed when <see cref="Execute" /> is called on the command.</param>
        /// <param name="canExecuteMethod">The delegate to be called when <see cref="CanExecute" /> is called on the command.</param>
        public RelayCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        ///     Method executed to determine whether or not the command can execute in its current state.
        /// </summary>
        /// <returns><c>true</c> if this instance can execute; otherwise, <c>false</c>.</returns>
        public bool CanExecute()
        {
            return CanExecute(null);
        }

        /// <summary>
        ///     Method executed to determine whether or not the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Information used by the command.</param>
        /// <returns>Returns true if this command can be executed, false otherwise.</returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecuteMethod == null) return true;
            return _canExecuteMethod(parameter);
        }

        /// <summary>
        ///     Method executed to determine whether or not the command can execute in its current state.
        /// </summary>
        /// <returns>Returns true if this command can be executed, false otherwise.</returns>
        public void Execute()
        {
            Execute(null);
        }

        /// <summary>
        ///     The method to be executed when the command is invoked.
        /// </summary>
        /// <param name="parameter">Information used by the command.</param>
        public void Execute(object parameter)
        {
            if (_executeMethod != null)
                _executeMethod(parameter);
        }

        /// <summary>
        ///     The method to be executed when the command is invoked.
        /// </summary>
        /// <param name="parameter">Information used by the command.</param>
        /// <example>
        ///     The following code holds a reference to the event handler. The myEventHandlerReference value should be stored
        ///     in an instance member to avoid it from being garbage collected.
        ///     <code>
        /// EventHandler myEventHandlerReference = new EventHandler(this.OnCanExecuteChanged);
        /// command.CanExecuteChanged += myEventHandlerReference;
        /// </code>
        /// </example>
        /// <remarks>
        ///     When subscribing to the <see cref="ICommand.CanExecuteChanged" /> event using
        ///     code (not when binding using XAML) will need to keep a hard reference to the event handler. This is to prevent
        ///     garbage collection of the event handler because the command implements the Weak Event pattern so it does not have
        ///     a hard reference to this handler. An example implementation can be seen in the CompositeCommand and
        ///     CommandBehaviorBase
        ///     classes. In most scenarios, there is no reason to sign up to the CanExecuteChanged event directly, but if you do,
        ///     you
        ///     are responsible for maintaining the reference.
        /// </remarks>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                //WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                //WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        /// <summary>
        ///     Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "The this keyword is used in the Silverlight version")]
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public void RaiseCanExecuteChanged()
        {
            //WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }
    }
}