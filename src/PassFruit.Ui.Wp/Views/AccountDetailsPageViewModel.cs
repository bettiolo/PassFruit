
using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using PassFruit.Contracts;
using PassFruit.Ui.Wp.Views.Controls;

namespace PassFruit.Ui.Wp.Views {

    public class AccountDetailsPageViewModel : Screen {

        private IAccount _account;

        public AccountDetailsPageViewModel() {
            Labels = new ObservableCollection<LabelViewModel>();
        }

        public ObservableCollection<LabelViewModel> Labels { get; private set; } 

        private Guid _accountId;
        public Guid AccountId {
            get { return _accountId; }
            set {
                _accountId = value;
                LoadAccount();
            }
        }

        public void LoadAccount() {
            var repository = Init.GetRepository();
            _account = repository.Accounts.GetById(_accountId);
            Notes = _account.Notes;
            NotifyOfPropertyChange(() => Account);
            NotifyOfPropertyChange(() => Title);
        }

        protected override void OnDeactivate(bool close) {
            SaveAccount();
            base.OnDeactivate(close);
        }

        private void SaveAccount() {
            _account.Notes = Notes;
            // _account.Save();
        }

        public string Title {
            get { return "PASSFRUIT | " + Account + " account details"; }
        }

        public string Account {
            get { return _account.Account; }
        }

        private string _notes;
        public string Notes {
            get { return _account.Notes; }
            set { _notes = value; NotifyOfPropertyChange(() => Notes); }
        }



    }

}
