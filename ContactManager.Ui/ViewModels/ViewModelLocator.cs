using System;
using System.Configuration;
using ContactsBook.Domain.Models;
using ContactsBook.Domain.Repositories.Interfaces;
using ContactsBook.Infrastructure.Repositories;
using ContactsBook.Ui.Views;
using ContactsBook.Ui.Views.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsBook.Ui.ViewModels
{
    public sealed class ViewModelLocator
    {
        private readonly IServiceProvider m_ServiceProvider;

        public ViewModelLocator()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            m_ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DatabaseSettings { ConnectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString });
            services.AddSingleton<IContactsRepository, SQLiteContactsRepository>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<EditorBase<Contact>, ContactEditorView>();
            services.AddTransient<ContactValidationViewModel>();
        }

        public MainWindowViewModel MainModel => m_ServiceProvider.GetService<MainWindowViewModel>();
        public ContactValidationViewModel ContactValidationModel => m_ServiceProvider.GetService<ContactValidationViewModel>();
    }
}
