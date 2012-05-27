using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp.Views.Controls.Fields {

    public class AccountTypeFieldViewModel : AccountFieldBase {

        private readonly IAccount _account;

        public AccountTypeFieldViewModel(IAccount account) {
            _account = account;
            AccountIcon = new AccountProviderIconViewModel(_account.Provider, 32);
        }

        public string AccountType {
            get { return _account.Provider.Name; }
        }

        private AccountProviderIconViewModel _accountIcon;
        public AccountProviderIconViewModel AccountIcon {
            get { return _accountIcon; }
            set {
                _accountIcon = value;
                NotifyOfPropertyChange(() => AccountIcon);
            }
        }

    }

}
