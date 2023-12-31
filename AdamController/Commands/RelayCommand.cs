﻿using System;
using System.Windows.Input;

namespace AdamController.Commands
{
    /// <summary>Provides an implementation of the <see cref="T:System.Windows.Input.ICommand" /> interface. </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> mExecute;
        private readonly Func<object, bool> mCanExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            mExecute = execute;
            mCanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return mCanExecute == null || mCanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            mExecute(parameter);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        #region Fields

        private readonly Action<T> mExecute;
        private readonly Predicate<T> mCanExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of DelegateCommand{T}/>.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
        /// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
        public RelayCommand(Action<T> execute) : this(execute, null){}

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            mExecute = execute ?? throw new ArgumentNullException(nameof(execute));
            mCanExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        /// <inheritdoc />
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter) => mCanExecute?.Invoke((T)parameter) ?? true;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <inheritdoc />
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            mExecute((T)parameter);
        }

        #endregion
    }
}
