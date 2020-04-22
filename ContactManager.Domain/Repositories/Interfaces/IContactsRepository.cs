using System.Collections.Generic;
using System.Threading.Tasks;
using ContactsBook.Domain.Models;

namespace ContactsBook.Domain.Repositories.Interfaces
{
    public interface IContactsRepository
    {
        /// <summary>
        /// Return all contacts.
        /// </summary>
        Task<IEnumerable<Contact>> GetAllAsync(LoadContactsFilter filter);

        /// <summary>
        /// Add new contact.
        /// </summary>
        Task AddAsync(Contact contact);

        /// <summary>
        /// Update existing contact.
        /// </summary>
        Task UpdateAsync(Contact contact);

        /// <summary>
        /// Delete contact by contact id.
        /// </summary>
        Task DeleteAsync(int id);
    }
}
