using DemoMVVM.Models;
using DemoMVVM.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ToolBox.Connections.Databases;
using ToolBox.MVVM.Commands;
using ToolBox.MVVM.ViewModels;

namespace DemoMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _nom, _prenom, _email;
        private ICommand _saveCommand;
        private ObservableCollection<ContactViewModel> _items;

        public string Nom
        {
            get
            {
                return _nom;
            }

            set
            {
                SetProperty(ref _nom, value);
            }
        }

        public string Prenom
        {
            get
            {
                return _prenom;
            }

            set
            {
                SetProperty(ref _prenom, value);
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                SetProperty(ref _email, value);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new DelegateCommand(Save, CanSave));
            }
        }

        public ObservableCollection<ContactViewModel> Items
        {
            get
            {
                return _items ?? (_items = LoadContacts());
            }
        }

        public MainViewModel()
        {
            Messenger<ContactViewModel>.Instance.Register("Delete", OnDeleteContact);
        }        

        private ObservableCollection<ContactViewModel> LoadContacts()
        {
            Connection connection = new Connection(@"Data Source=AW-BRIAREOS\SQL2016DEV;Initial Catalog=DemoMVVM;Integrated Security=True;", SqlClientFactory.Instance);
            Command command = new Command("Select Id, LastName, FirstName, Email from Contact");

            return new ObservableCollection<ContactViewModel>(connection.ExecuteReader(command, (dr) => new Contact() { Id = (int)dr["Id"], Nom = (string)dr["LastName"], Prenom = (string)dr["FirstName"], Email = (string)dr["Email"] }).Select(c => new ContactViewModel(c)));
        }

        private void Save()
        {
            Connection connection = new Connection(@"Data Source=AW-BRIAREOS\SQL2016DEV;Initial Catalog=DemoMVVM;Integrated Security=True;", SqlClientFactory.Instance);
            Command command = new Command("SP_AddContact", true);
            command.AddParameter("LastName", Nom);
            command.AddParameter("FirstName", Prenom);
            command.AddParameter("Email", Email);

            int Id = (int)connection.ExecuteScalar(command);

            Items.Add(new ContactViewModel(new Contact() { Id = Id, Nom = Nom, Prenom = Prenom, Email = Email }));

            Nom = Prenom = Email = null;
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Nom) &&
                !string.IsNullOrWhiteSpace(Prenom) &&
                !string.IsNullOrWhiteSpace(Email);
        }

        private void OnDeleteContact(ContactViewModel contactViewModel)
        {
            Items.Remove(contactViewModel);
        }
    }
}
