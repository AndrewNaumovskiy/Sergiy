using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sergiy.Models
{
    public class Account : ViewModelBase
    {
        public enum AccountStatus
        {
            None,
            CorrectCreds,
            WrongCreds
        }

        private AccountStatus _status;

        public string Login { get; set; }
        public string Password { get; set; }
        public AccountStatus Status 
        { 
            get => _status; 
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public string Description { get; set; }

        public Account(string login, string password)
        {
            Login = login;
            Password = password;
            Status = AccountStatus.None;
            Description = "";
        }
    }
}
