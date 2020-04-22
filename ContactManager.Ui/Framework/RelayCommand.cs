using System;
using System.Windows.Input;

namespace ContactsBook.Ui.Framework
{
    public sealed class RelayCommand : ICommand
    {
        private readonly Action m_Execute;
        private readonly Func<bool> m_CanExecute;
        
        public bool IsExecuting { get; private set; }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            m_Execute = execute ?? throw new ArgumentNullException("execute");
            m_CanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return m_CanExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            IsExecuting = true;
            
            m_Execute.Invoke();

            IsExecuting = false;
        }
    }
}
