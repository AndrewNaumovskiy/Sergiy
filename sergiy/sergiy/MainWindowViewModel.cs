using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using sergiy.Models;
using static sergiy.Models.Account;

namespace sergiy
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private RelayCommand _uploadProxyCommand;
        private RelayCommand _uploadAccountsCommand;
        private RelayCommand _startCommand;


        private ObservableCollection<UploadProxy> _uploadProxies;
        private ObservableCollection<Account> _accounts;

        private bool _sortStatus;

        #endregion

        #region Properties

        public ObservableCollection<UploadProxy> UploadProxies 
        { 
            get => _uploadProxies;
            set
            {
                _uploadProxies = value;
                OnPropertyChanged(nameof(UploadProxies));
            }
        }

        public ObservableCollection<Account> Accounts 
        { 
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public ObservableCollection<Account> AccountsCopy { get; set; }

        public bool SortStatus 
        {
            get => _sortStatus;
            set
            {
                _sortStatus = value;
                Filter();
                OnPropertyChanged(nameof(SortStatus));
            }
        }


        #endregion

        #region Commands

        public RelayCommand UploadProxyCommand 
        { 
            get
            {
                if (_uploadProxyCommand == null)
                    _uploadProxyCommand = new RelayCommand((param) => UploadProxyAction());

                return _uploadProxyCommand;
            } 
        }

        public RelayCommand UploadAccountsCommand
        {
            get
            {
                if (_uploadAccountsCommand == null)
                    _uploadAccountsCommand = new RelayCommand((param) => UploadAccountsAction());

                return _uploadAccountsCommand;
            }
        }

        public RelayCommand StartCommand 
        { 
            get
            {
                if (_startCommand == null)
                    _startCommand = new RelayCommand((param) => StartAction());

                return _startCommand;
            } 
        }

        #endregion

        public MainWindowViewModel()
        {
            UploadProxies = new ObservableCollection<UploadProxy>();
            Accounts = new ObservableCollection<Account>();
        }
        #region Logic

        private async void UploadProxyAction()
        {
            var kek = await DataProxy.LoadProxies();
            foreach (var item in kek.ToList())
                UploadProxies.Add(item);
        }

        private async void UploadAccountsAction()
        {
            var kek = await DataProxy.LoadAccounts();

            foreach (var item in kek.ToList())
                Accounts.Add(item);

            AccountsCopy = new ObservableCollection<Account>(Accounts);
        }

        private void StartAction()
        {
            Accounts[0].Status = Account.AccountStatus.CorrectCreds;
            Accounts[1].Status = Account.AccountStatus.WrongCreds;
            Accounts[2].Status = Account.AccountStatus.CorrectCreds;
        }

        private void Filter()
        {
            Accounts?.Clear();
            if(_sortStatus == false)
            {
                foreach (var item in AccountsCopy)
                    Accounts.Add(item);
            }
            else
            {
                var temp = AccountsCopy.Where(x => x.Status == AccountStatus.CorrectCreds).ToList();
                foreach (var item in temp)
                    Accounts.Add(item);
            }
        }

        #endregion
    }
}
