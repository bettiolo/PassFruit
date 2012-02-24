using System;
using Caliburn.Micro;
using Microsoft.Phone.UserData;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp.Views.Controls {

    public class AccountViewModel : PropertyChangedBase {

        public AccountViewModel(IAccount account) {
            if (account == null) {
                return;
            }
            AccountIcon = new AccountProviderIconViewModel(account.Provider, 64);
            Id = account.Id;
            ProviderName = account.Provider.Name;
            Account = account.GetAccountName();
        }

        private Guid _id;
        public Guid Id {
            get { return _id; }
            set {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        private string _providerName;
        public string ProviderName {
            get { return _providerName; }
            set {
                _providerName = value;
                NotifyOfPropertyChange(() => ProviderName);
            }
        }

        private string _account;
        public string Account {
            get { return _account; }
            set {
                _account = value;
                NotifyOfPropertyChange(() => Account);
            }
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

    public class AccountViewModelSample : AccountViewModel {

        public AccountViewModelSample() : base(null) {
            AccountIcon = new AccountProviderIconViewModel(null, 64);
            Id = Guid.NewGuid();
            ProviderName = "Provider";
            Account = "account - info@example.com";
        }
    }

}
