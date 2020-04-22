using System.Windows;
using ContactsBook.Ui.ViewModels;
using ContactsBook.Ui.Framework;

namespace ContactsBook.Ui.Views.Base
{
    public abstract class EditorBase<TInstance> : Window where TInstance : class, new()
    {
        protected EditorBase()
        {
            // Set the window up like a dialog
            WindowStyle = WindowStyle.SingleBorderWindow;
            ResizeMode = ResizeMode.NoResize;
            DataContextChanged += delegate { InitValidCommand(); };
        }

        public ValidationViewModel<TInstance> ValidationModel
        {
            get => DataContext as ValidationViewModel<TInstance>;
            set => DataContext = value;
        }

        private void InitValidCommand()
        {
            if (ValidationModel != null)
            {
                ValidationModel.ValidCommand = new RelayCommand(ValidCommandExecute, CanValidCommandExecute);
            }
        }

        private bool CanValidCommandExecute() => ValidationModel?.IsValid == true;

        private void ValidCommandExecute()
        {
            DialogResult = true;
        }
    }
}
