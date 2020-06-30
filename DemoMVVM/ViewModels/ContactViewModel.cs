using DemoMVVM.Models;
using DemoMVVM.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;
using ToolBox.Connections.Databases;
using ToolBox.MVVM.Commands;
using ToolBox.MVVM.ViewModels;

namespace DemoMVVM.ViewModels
{
    public class ContactViewModel : EntityViewModel<Contact>
    {
        private string _nom, _prenom, _email;
        private ICommand _updateCommand, _deleteCommand;

        public int Id
        {
            get { return Entity.Id; }
        }

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

        public ICommand UpdateCommand
        {
            get
            {
                return _updateCommand ?? (_updateCommand = new DelegateCommand(Update, CanUpdate));
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new DelegateCommand(Delete));
            }
        }

        public ContactViewModel(Contact entity) : base(entity)
        {
            Nom = entity.Nom;
            Prenom = entity.Prenom;
            Email = entity.Email;
        }

        private void Update()
        {
            Entity.Nom = Nom;
            Entity.Prenom = Prenom;
            Entity.Email = Email;

            Connection connection = new Connection(@"Data Source=AW-BRIAREOS\SQL2016DEV;Initial Catalog=DemoMVVM;Integrated Security=True;", SqlClientFactory.Instance);
            Command command = new Command("SP_UpdateContact", true);
            command.AddParameter("Id", Id);
            command.AddParameter("LastName", Nom);
            command.AddParameter("FirstName", Prenom);
            command.AddParameter("Email", Email);

            connection.ExecuteNonQuery(command);
        }

        private bool CanUpdate()
        {
            return Entity.Nom != Nom ||
                Entity.Prenom != Prenom ||
                Entity.Email != Email;
        }

        private void Delete()
        {
            Connection connection = new Connection(@"Data Source=AW-BRIAREOS\SQL2016DEV;Initial Catalog=DemoMVVM;Integrated Security=True;", SqlClientFactory.Instance);
            Command command = new Command("SP_DeleteContact", true);
            command.AddParameter("Id", Id);

            connection.ExecuteNonQuery(command);

            Messenger<ContactViewModel>.Instance.Send("Delete", this);
        }
    }
}
