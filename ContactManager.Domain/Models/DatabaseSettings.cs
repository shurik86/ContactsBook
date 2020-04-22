using ContactsBook.Domain.Common;

namespace ContactsBook.Domain.Models
{
    public sealed class DatabaseSettings
    {
        public string ConnectionString { get; set; } = Constants.DefaultConnectionString;
    }
}
