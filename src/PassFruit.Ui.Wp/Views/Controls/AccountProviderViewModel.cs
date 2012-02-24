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

namespace PassFruit.Ui.Wp.Views.Controls {

    public class AccountProviderViewModel : PropertyChangedBase {

        public IProvider Provider { get; private set; }

        public AccountProviderViewModel(IProvider provider) {
            Provider = provider;
            LoadAccountProvider();
        }

        private void LoadAccountProvider() {
            NotifyOfPropertyChange(() => ProviderName);
            ProviderIcon = new AccountProviderIconViewModel(Provider, 64);
            NotifyOfPropertyChange(() => ProviderIcon);
            NotifyOfPropertyChange(() => ProviderUrl);
        }

        public string ProviderName {
            get { return Provider.Name; }
        }        
        
        public string ProviderUrl {
            get { return Provider.Url; }
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
