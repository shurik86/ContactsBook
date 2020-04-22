using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ContactsBook.Ui.Framework.Interfaces;
using ContactsBook.Ui.Framework.Utils;
using log4net.Core;

namespace ContactsBook.Ui.Framework
{
    public class AsyncCommand<T> : AsyncCommand, IAsyncCommand<T>
    {
        private readonly Func<T, Task> m_Execute;
        private readonly Func<T, bool> m_CanExecute;
        
        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null)
                :base(errorHandler)
        {
            m_Execute = execute ?? throw new ArgumentNullException("execute");
            m_CanExecute = canExecute;
        }

        public bool CanExecute(T parameter)
        {
            return m_CanExecute?.Invoke(parameter) ?? true;
        }

        public async Task ExecuteAsync(T parameter)
        {
            IsExecuting = true;
            await m_Execute(parameter);
            IsExecuting = false;
        }

        public override bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        public override void Execute(object parameter)
        {
            ExecuteAsync((T)parameter).FireAndForgetSafeAsync(ErrorHandler);
        }
    }
    public class AsyncCommand : IAsyncCommand
    {
        private readonly Func<Task> m_Execute;
        private readonly Func<bool> m_CanExecute;
        
        protected IErrorHandler ErrorHandler { get; }

        public bool IsExecuting { get; protected set; }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        protected AsyncCommand(IErrorHandler errorHandler = null)
        {
            ErrorHandler = errorHandler;
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            m_Execute = execute ?? throw new ArgumentNullException("execute");
            m_CanExecute = canExecute;
            ErrorHandler = errorHandler;
        }

        public bool CanExecute()
        {
            return m_CanExecute?.Invoke() ?? true;
        }

        public async Task ExecuteAsync()
        {
            IsExecuting = true;
            await m_Execute();
            IsExecuting = false;
        }
        
        public virtual bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        public virtual void Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync(ErrorHandler);
        }
    }
}
