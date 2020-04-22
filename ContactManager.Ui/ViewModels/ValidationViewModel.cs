using System.ComponentModel;
using System.Windows.Input;
using ContactsBook.Domain.Common;

namespace ContactsBook.Ui.ViewModels
{
    public abstract class ValidationViewModel<TInstance> : NotifiableObject, IDataErrorInfo where TInstance : class, new()
    {
        private ICommand m_ValidCommand;
        public ICommand ValidCommand
        {
            get => m_ValidCommand;
            set => SetPropertyValue(ref m_ValidCommand, value);
        }

        public abstract bool IsValid { get; }
        
        public abstract TInstance Instance { get; set; }
        
        /// <summary>Gets the error message for the property with the given name.</summary>
        /// <param name="columnName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public abstract string this[string columnName] { get; }

        /// <summary>Gets an error message indicating what is wrong with this object.</summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public string Error { get; protected set; } = string.Empty;
    }
}
