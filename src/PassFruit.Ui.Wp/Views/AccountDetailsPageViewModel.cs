
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using PassFruit.Contracts;
using PassFruit.Ui.Wp.Views.Controls;

namespace PassFruit.Ui.Wp.Views {

    public class AccountDetailsPageViewModel : Screen {

        private IAccount _account;
        private bool _passwordLoaded;

        public AccountDetailsPageViewModel() {

        }

        public IEnumerable<TagViewModel> Tags {
            get { return _account.AccountTags.Select(accountTag => new TagViewModel(accountTag)); }
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
            _passwordLoaded = false;
            var repository = Init.GetRepository();
            _account = repository.Accounts[_accountId];
            NotifyOfPropertyChange(() => AccountName);
            NotifyOfPropertyChange(() => Title);
            NotifyOfPropertyChange(() => Email);
            NotifyOfPropertyChange(() => IsEmailEnabled);
            NotifyOfPropertyChange(() => UserName);
            NotifyOfPropertyChange(() => IsUserNameEnabled);
            NotifyOfPropertyChange(() => Notes);
            NotifyOfPropertyChange(() => Tags);
            Password = "*****";
            AccountIcon = new AccountProviderIconViewModel(_account, 32);
        }

        public void PopulatePassword() {
            if (!_passwordLoaded) {
                Password = _account.GetPassword();
            }
        }

        protected override void OnDeactivate(bool close) {
            SaveAccount();
            base.OnDeactivate(close);
        }

        private void SaveAccount() {
            if (_passwordLoaded) {
                _account.SetPassword(Password);
            }
            _account.Save();
        }

        public Visibility IsUserNameEnabled {
            get {
                return _account is IAccountHasUserName
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        public Visibility IsEmailEnabled {
            get {
                return _account is IAccountHasEmail
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        public string Title {
            get { return "PASSFRUIT | " + AccountName; }
        }

        public string AccountName {
            get { return _account.AccountName; }
        }

        public string Email {
            get {
                var accountWithEmail = _account as IAccountHasEmail;
                return accountWithEmail != null 
                    ? accountWithEmail.Email 
                    : "";
            }
            set {
                var accountWithEmail = _account as IAccountHasEmail;
                if (accountWithEmail == null) return;

                accountWithEmail.Email = value;
                NotifyOfPropertyChange(() => Email);
            }
        }

        public string UserName {
            get {
                var accountWithUserName = _account as IAccountHasUserName;
                return accountWithUserName != null
                    ? accountWithUserName.UserName
                    : "";
            }
            set {
                var accountWithUserName = _account as IAccountHasUserName;
                if (accountWithUserName == null) return;

                accountWithUserName.UserName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Notes {
            get { return _account.Notes; }
            set { _account.Notes = value; NotifyOfPropertyChange(() => Notes); }
        }

        public void CopyPassword() {
            Clipboard.SetText(_account.GetPassword());
            MessageBox.Show("Password copied");
        }

        public void ShowPassword() {
            MessageBox.Show("Password is '" + _account.GetPassword() + "'");
        }

        public void CopyUserName() {
            Clipboard.SetText(UserName);
            MessageBox.Show("Username copied: '" + UserName + "'");
        }

        public void CopyEmail() {
            Clipboard.SetText(Email);
            MessageBox.Show("Email copied: '" + Email + "'");
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

        private string _password;

        public string Password {
            get { return _password; }
            set { _password = value; NotifyOfPropertyChange(() => Password);} }
        }

}
