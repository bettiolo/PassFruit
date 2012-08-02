
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using PassFruit.Contracts;
using PassFruit.Ui.Wp.Views.Controls;
using PassFruit.Ui.Wp.Views.Controls.Fields;

namespace PassFruit.Ui.Wp.Views {

    public class AccountDetailsPageViewModel : Screen {

        private IAccount _account;

        public AccountDetailsPageViewModel() {

        }

        public IEnumerable<TagViewModel> Tags {
            get { return _account.Tags.Select(accountTag => new TagViewModel(accountTag)); }
        }

        private Guid _accountId;
        public Guid AccountId {
            get { return _accountId; }
            set {
                _accountId = value;
                LoadAccount();
            }
        }

        public void LoadAccount() {
            //_passwordLoaded = false;
            var dataStore = Init.GetDataStore();
            _account = new Accounts(dataStore).GetById(_accountId);
            NotifyOfPropertyChange(() => AccountName);
            NotifyOfPropertyChange(() => Title);
            //NotifyOfPropertyChange(() => Email);
            //NotifyOfPropertyChange(() => IsEmailEnabled);
            //NotifyOfPropertyChange(() => UserName);
            //NotifyOfPropertyChange(() => IsUserNameEnabled);
            //NotifyOfPropertyChange(() => Notes);
            NotifyOfPropertyChange(() => Tags);
            //Password = "*****";
            DisplayFields = new ObservableCollection<AccountFieldBase>();
            DisplayFields.Add(new AccountTypeFieldViewModel(_account));
            DisplayFields.Add(new PasswordFieldViewModel(_account));
            foreach(var field in _account.Fields.Where(f => f != null)) {
                DisplayFields.Add(new GenericAccountFieldViewModel(field));
            }
            DisplayFields.Add(new NotesFieldViewModel(_account));
        }

        //public void PopulatePassword() {
        //    if (!_passwordLoaded) {
        //        Password = _account.GetPassword();
        //    }
        //}

        protected override void OnDeactivate(bool close) {
            SaveAccount();
            base.OnDeactivate(close);
        }

        private void SaveAccount() {
            // _account.Save();
            throw new NotSupportedException();
        }

        //public Visibility IsUserNameEnabled {
        //    get {
        //        return _account.Provider.HasUserName
        //            ? Visibility.Visible
        //            : Visibility.Collapsed;
        //    }
        //}

        //public Visibility IsEmailEnabled {
        //    get {
        //        return _account.Provider.HasEmail
        //            ? Visibility.Visible
        //            : Visibility.Collapsed;
        //    }
        //}

        public string Title {
            get { return "PASSFRUIT | " + AccountName; }
        }

        public string AccountName {
            get { return _account.GetAccountName(); }
        }

        //public string Email {
        //    get {
        //        var email = _account.GetDefaultField<string>(FieldTypeKey.Email);
        //        return (email != null)
        //            ? email.Value
        //            : "";
        //    }
        //    set {
        //        _account.SetField(FieldTypeKey.Email, value);

        //        NotifyOfPropertyChange(() => Email);
        //        NotifyOfPropertyChange(() => Title);
        //    }
        //}

        //public string UserName {
        //    get {
        //        var userName = _account.GetDefaultField<string>(FieldTypeKey.UserName);
        //        return (userName != null)
        //            ? userName.Value
        //            : "";
        //    }
        //    set {
        //        _account.SetField(FieldTypeKey.UserName, value);

        //        NotifyOfPropertyChange(() => UserName);
        //        NotifyOfPropertyChange(() => Title);
        //    }
        //}

        public string Notes {
            get { return _account.Notes; }
            set { _account.Notes = value; NotifyOfPropertyChange(() => Notes); }
        }



        //public void CopyUserName() {
        //    Clipboard.SetText(UserName);
        //    MessageBox.Show("Username copied: '" + UserName + "'");
        //}

        //public void CopyEmail() {
        //    Clipboard.SetText(Email);
        //    MessageBox.Show("Email copied: '" + Email + "'");
        //}

        private string _password;
        public string Password {
            get { return _password; }
            set { _password = value; NotifyOfPropertyChange(() => Password); }
        }

        public ObservableCollection<AccountFieldBase> DisplayFields { get; private set; } 

    }

}
