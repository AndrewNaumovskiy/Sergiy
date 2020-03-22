using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using sergiy.Models;
using static sergiy.Models.Account;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace sergiy
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        IWebDriver driver;


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
            Thread kek = new Thread(meow);
            kek.Start();
        }

        private async void meow()
        {
            await Task.Run(() => 
            {
                driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://auth.riotgames.com/login#client_id=rso-web-client-prod&login_hint=na&redirect_uri=https%3A%2F%2Flogin.leagueoflegends.com%2Foauth2-callback&response_type=code&scope=openid&state=&ui_locales=en-us");

                Thread.Sleep(5000);

                IWebElement login = driver.FindElement(By.Name("username"));
                login.SendKeys("IamHoustonJoker");

                IWebElement password = driver.FindElement(By.Name("password"));
                password.SendKeys("Volo4nuk1993");

                IWebElement submit = driver.FindElement(By.ClassName("mobile-button"));
                submit.Click();

                Thread.Sleep(2000);

                driver.Navigate().GoToUrl("https://account.riotgames.com/account");

                IWebElement passwordCheck = driver.FindElement(By.ClassName("field__form-input"));
                passwordCheck.SendKeys("Volo4nuk1993");

                submit = driver.FindElement(By.ClassName("mobile-button"));
                submit.Click();


                if (driver.FindElements(By.ClassName("grid-direction__column")).Count != 0)
                {
                    //MessageBox.Show("BINDED");
                }
                

                driver.Navigate().GoToUrl("https://na.leagueoflegends.com/en-us/");

                Thread.Sleep(2000);

                var continuew = driver.FindElement(By.ClassName("continue"));
                continuew.Click();
                
                Thread.Sleep(2000);

               
                //var kek = driver.FindElement(By.ClassName(""));
            });
            
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
