using ContactsBook.Domain.Common;

namespace ContactsBook.Domain.Models
{
    public sealed class LoadContactsFilter : NotifiableObject
    {
        public string SearchString { get; set; } = string.Empty;
    }
}
