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
using PassFruit.AccountProviders;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp.Views.Controls {

    public class AccountProviderViewModel : PropertyChangedBase {

        public IAccountProvider AccountProvider { get; private set; };

        public AccountProviderViewModel(IAccountProvider accountProvider) {
            AccountProvider = accountProvider;
            LoadAccountProvider();
        }

        private void LoadAccountProvider() {
            NotifyOfPropertyChange(() => ProviderName);
            ProviderIcon = new AccountProviderIconViewModel(AccountProvider, 64);
            NotifyOfPropertyChange(() => ProviderIcon);
            NotifyOfPropertyChange(() => ProviderUrl);
        }

        public string ProviderName {
            get { return AccountProvider.Name; }
        }        
        
        public string ProviderUrl {
            get { return AccountProvider.Url; }
        }

        private AccountProviderIconViewModel _providerIcon;
        public AccountProviderIconViewModel ProviderIcon {
            get { return _providerIcon; }
            set {
                _providerIcon = value;
                NotifyOfPropertyChange(() => ProviderIcon);
            }
        }

    }

}
