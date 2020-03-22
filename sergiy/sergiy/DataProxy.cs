using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sergiy.Models;
using Microsoft.Win32;
using WinForms = System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace sergiy
{
    public static class DataProxy
    {
        public static ObservableCollection<UploadProxy> UploadProxytemp = new ObservableCollection<UploadProxy>();
        public static ObservableCollection<Account> AccountsTemp = new ObservableCollection<Account>();

        public async static Task<ObservableCollection<UploadProxy>> LoadProxies()
        {
            UploadProxytemp?.Clear();

            string _filePath = "";
            string[] temp = { };

            WinForms.OpenFileDialog fileDialog = new WinForms.OpenFileDialog();

            WinForms.DialogResult result = fileDialog.ShowDialog();

            await Task.Run(async () =>
            {
                if (result == WinForms.DialogResult.OK)
                {
                    _filePath = fileDialog.FileName;

                    using (var sr = new StreamReader(_filePath))
                    {
                        var fileText = await sr.ReadToEndAsync();
                        temp = fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    }
                }

                foreach (var item in temp.ToList())
                {
                    if (item.Contains(':'))
                    {
                        int port = int.TryParse(item.Split(':')[1], out port) ? port : 0;
                        UploadProxytemp.Add(new UploadProxy(item.Split(':')[0], port));
                    }
                }
            });

            return UploadProxytemp;
        }

        public async static Task<ObservableCollection<Account>> LoadAccounts()
        {
            AccountsTemp?.Clear();

            string _filePath = "";
            string[] temp = { };

            WinForms.OpenFileDialog fileDialog = new WinForms.OpenFileDialog();

            WinForms.DialogResult result = fileDialog.ShowDialog();
           

            
            await Task.Run(async () => 
            {
                if (result == WinForms.DialogResult.OK)
                {
                    _filePath = fileDialog.FileName;

                    using (var sr = new StreamReader(_filePath))
                    {
                        var fileText = await sr.ReadToEndAsync();
                        temp = fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    }
                }

                foreach (var item in temp.ToList())
                {
                    if (item.Contains(':'))
                    {
                        AccountsTemp.Add(new Account(item.Split(':')[0], item.Split(':')[1]));
                    }
                    else
                    {
                        AccountsTemp.Add(new Account(item, item));
                    }
                }
            });
            

            return AccountsTemp;
        }
    }
}
