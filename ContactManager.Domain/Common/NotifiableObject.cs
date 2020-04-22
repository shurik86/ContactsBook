using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ContactsBook.Domain.Common
{
    /// <summary>
    /// Represents base INotifyPropertyChanged implementation.
    /// </summary>
    public class NotifiableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">Changed property name.</param>
        protected void SetPropertyValue<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(oldValue, default(T)) && Equals(newValue, default(T)) || Equals(oldValue, newValue))
                return;

            oldValue = newValue;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
