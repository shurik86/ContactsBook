using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ContactsBook.Domain.Models;
using ContactsBook.Domain.Utils;

namespace ContactsBook.Ui.ViewModels
{
    public sealed class ContactValidationViewModel : ValidationViewModel<Contact>
    {
        #region Fields

        private readonly Regex m_EmailRegex = new Regex(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$");

        private int m_Id;
        private string m_Name;
        private string m_Surname;
        private string m_PhoneNumber;
        private string m_Email;

        #endregion

        public override Contact Instance
        {
            get => new Contact
                       {
                           Id = m_Id,
                           Name = Name?.Trim(),
                           Surname = Surname?.Trim(),
                           PhoneNumber = PhoneNumber?.Trim(),
                           Email = Email?.Trim()
                       };
            set
            {
                if (value != null)
                {
                    m_Id = value.Id;
                    Name = value.Name;
                    Surname = value.Surname;
                    PhoneNumber = value.PhoneNumber;
                    Email = value.Email;
                }
            }
        }
        
        public string Name
        {
            get => m_Name;
            set => SetPropertyValue(ref m_Name, value);
        }
        
        public string Surname
        {
            get => m_Surname;
            set => SetPropertyValue(ref m_Surname, value);
        }

        public string PhoneNumber
        {
            get => m_PhoneNumber;
            set => SetPropertyValue(ref m_PhoneNumber, value);
        }

        public string Email
        {
            get => m_Email;
            set => SetPropertyValue(ref m_Email, value);
        }

        private bool IsEmailValid => m_EmailRegex.IsMatch(Email.Trim());
        private bool IsPhoneNumberValid => PhoneNumber.Length == 12 && PhoneNumber.All(char.IsDigit);

        public override bool IsValid => !string.IsNullOrWhiteSpace(Name) && 
                                        !string.IsNullOrWhiteSpace(PhoneNumber) && 
                                        IsPhoneNumberValid && 
                                        (string.IsNullOrWhiteSpace(Email) || IsEmailValid);

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public override string this[string columnName]
        {
			get
            {
                Error = string.Empty;

                if (IsColumn(columnName, () => Name) && string.IsNullOrWhiteSpace(Name))
                {
                    Error = "Name is required!";
                }
                else if (IsColumn(columnName, () => PhoneNumber))
                {
                    if (string.IsNullOrWhiteSpace(PhoneNumber))
                    {
                        Error = "Phone number is required!";
                    }
                    else if (!IsPhoneNumberValid)
                    {
                        Error = "Please enter valid phone number!";
                    }
                }
                else if (IsColumn(columnName, () => Email) && !string.IsNullOrWhiteSpace(Email) && !IsEmailValid)
                {
                    Error = "Please enter valid email address!";
                }
                
                return Error;
            }
		}

        private static bool IsColumn<T>(string columnName, Expression<Func<T>> expression)
        {
            return PropertyNameProvider.GetPropertyName(expression).Equals(columnName, StringComparison.Ordinal);
        }
    }
}
