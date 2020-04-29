using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ContactsBook.Domain.Models;
using ContactsBook.Domain.Repositories.Interfaces;
using ContactsBook.Infrastructure.Utils;
using SQLite;

namespace ContactsBook.Infrastructure.Repositories
{
    public sealed class SQLiteContactsRepository : IContactsRepository
    {
        private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.SharedCache;
        private readonly SQLiteAsyncConnection m_DataContext;

        public SQLiteContactsRepository(DatabaseSettings settings)
        {
            m_DataContext = new SQLiteAsyncConnection(settings.ConnectionString, Flags);
            InitializeDatabaseAsync(settings.ConnectionString).SafeFireAndForget(false);
        }

        /// <summary>
        /// Return all contacts.
        /// </summary>
        public async Task<IEnumerable<Contact>> GetAllAsync(LoadContactsFilter filter)
        {
            return !string.IsNullOrWhiteSpace(filter.SearchString) 
                           ? await GetAllWithFilterByStringAsync(filter) 
                           : await GetAllWithoutFilterAsync();
        }

        /// <summary>
        /// Return all contacts by filter.
        /// </summary>
        private async Task<IEnumerable<Contact>> GetAllWithFilterByStringAsync(LoadContactsFilter filter)
        {
            var searchString = filter.SearchString?.ToLower() ?? string.Empty;
            return await m_DataContext.Table<Contact>()
                                      .Where(c => c.Name.Contains(searchString) ||
                                                  c.Surname.Contains(searchString) ||
                                                  c.PhoneNumber.Contains(searchString) ||
                                                  c.Email.Contains(searchString))
                                      .ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Return all contacts.
        /// </summary>
        private async Task<IEnumerable<Contact>> GetAllWithoutFilterAsync()
        {
            return await m_DataContext.Table<Contact>().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Add new contact.
        /// </summary>
        public async Task AddAsync(Contact contact)
        {
            await m_DataContext.InsertAsync(contact).ConfigureAwait(false);
        }

        /// <summary>
        /// Update existing contact.
        /// </summary>
        public async Task UpdateAsync(Contact contact)
        {
            await m_DataContext.UpdateAsync(contact).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete contact by contact id.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var noteItem = await m_DataContext.Table<Contact>().FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false); ;
            await m_DataContext.DeleteAsync(noteItem).ConfigureAwait(false); ;
        }

        private async Task InitializeDatabaseAsync(string database)
        {
            if (!File.Exists(database))
            {
                await m_DataContext.CreateTableAsync<Contact>().ConfigureAwait(false);
            }
        }
    }
}
