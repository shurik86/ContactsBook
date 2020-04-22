using ContactsBook.Domain.Common;
using SQLite;

namespace ContactsBook.Domain.Models
{
    [Table("Contacts")]
    public sealed class Contact
    {
        /// <summary>
        /// Gets or sets contact id.
        /// </summary>
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets contact name.
        /// </summary>
        [NotNull]
        [MaxLength(Constants.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets contact surname.
        /// </summary>
        [MaxLength(Constants.MaxSurnameLength)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        [NotNull]
        [Unique]
        [MaxLength(Constants.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [MaxLength(Constants.MaxEmailLength)]
        public string Email { get; set; }
    }
}
