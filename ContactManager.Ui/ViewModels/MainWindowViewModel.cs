using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Models;
using ContactsBook.Domain.Repositories.Interfaces;
using ContactsBook.Ui.Framework;
using ContactsBook.Ui.Framework.Interfaces;
using ContactsBook.Ui.Views.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsBook.Ui.ViewModels
{
    public sealed class MainWindowViewModel : NotifiableObject
    {
        private readonly IContactsRepository m_ContactsRepository;
        private readonly IServiceProvider m_ServiceProvider;
        private readonly LoadContactsFilter m_SearchFilter = new LoadContactsFilter();
        private string m_FilterString;
        private Contact m_SelectedContact;

        public MainWindowViewModel(IContactsRepository repository, IServiceProvider serviceProvider)
        {
            m_ContactsRepository = repository;
            m_ServiceProvider = serviceProvider;

            LoadCommand = new AsyncCommand(LoadCommandExecuteAsync);
            AddCommand = new AsyncCommand(AddCommandExecuteAsync);
            EditCommand = new AsyncCommand<Contact>(EditCommandExecuteAsync, CanEditCommandExecute);
            DeleteCommand = new AsyncCommand<Contact>(DeleteCommandExecuteAsync, CanDeleteCommandExecute);
        }

        #region Commands
        
        public IAsyncCommand LoadCommand { get; }
        public IAsyncCommand AddCommand { get; }
        public IAsyncCommand<Contact> EditCommand { get; }
        public IAsyncCommand<Contact> DeleteCommand { get; }
        
        #endregion

        public string FilterString
        {
            get => m_FilterString;
            set => SetPropertyValue(ref m_FilterString, value);
        }

        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();

        public Contact SelectedContact
        {
            get => m_SelectedContact;
            set => SetPropertyValue(ref m_SelectedContact, value);
        }

        private bool CanEditCommandExecute(Contact contact) => contact != null;

        private bool CanDeleteCommandExecute(Contact contact) => contact != null;

        private async Task LoadCommandExecuteAsync() => await LoadDataAsync();

        private async Task AddCommandExecuteAsync()
        {
            var editor = m_ServiceProvider.GetRequiredService<EditorBase<Contact>>();
            if (editor.ShowDialog() == true)
            {
                await m_ContactsRepository.AddAsync(editor.ValidationModel.Instance);
                await LoadDataAsync();
            }
        }
        
        private async Task EditCommandExecuteAsync(Contact contact)
        {
            var editor = m_ServiceProvider.GetRequiredService<EditorBase<Contact>>();
            editor.ValidationModel.Instance = contact;
            if (editor.ShowDialog() == true)
            {
                await m_ContactsRepository.UpdateAsync(editor.ValidationModel.Instance);
                await LoadDataAsync();
            }
        }
        
        private async Task DeleteCommandExecuteAsync(Contact contact)
        {
            await m_ContactsRepository.DeleteAsync(contact.Id);
            Contacts.Remove(contact);
        }
        
        private async Task LoadDataAsync()
        {
            m_SearchFilter.SearchString = m_FilterString;
            var contacts = await m_ContactsRepository.GetAllAsync(m_SearchFilter);
            Contacts.Clear();
            contacts.ToList().ForEach(Contacts.Add);
        }
    }
}
