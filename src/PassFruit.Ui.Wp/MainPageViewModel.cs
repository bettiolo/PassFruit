using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using IsolatedStorageExtensions;
using PassFruit.Contracts;
using PassFruit.DataStore.Contracts;
using PassFruit.DataStore.Tests.FakeData;
using PassFruit.Ui.Wp.Views;
using PassFruit.Ui.Wp.Views.Controls;

namespace PassFruit.Ui.Wp {

    public class MainPageViewModel : Screen {

        private readonly IDataStore _dataStore;

        INavigationService _navigationService;

        private Accounts _accounts;

        public ObservableCollection<TagViewModel> Tags { get; private set; }

        public ObservableCollection<AccountViewModel> Accounts { get; private set; }

        public MainPageViewModel(INavigationService navigationService) {
            Tags = new ObservableCollection<TagViewModel>();
            Accounts = new ObservableCollection<AccountViewModel>();
            _navigationService = navigationService;

            var selectedDataStore = IsolatedStorageHelper.GetApplicationSetting("dataStore") as IDataStore;
            if (selectedDataStore == null) {
                var app = Application.Current as App;
                if (app != null) {
                    _dataStore = app.DataStores.GetSelectedDataStore();
                } else {
                    var init = new Init();
                    _dataStore = init.GetDataStores().GetSelectedDataStore();
                }
                var fakeData = new FakeDataGenerator();
                fakeData.GenerateFakeData(_dataStore);
            } else {
                _dataStore = selectedDataStore;
            }

            _accounts = new Accounts(_dataStore);

            UpdateUi();

            // _dataStore.OnSaved += RepositoryChanged;

        }

        //private void RepositoryChanged(object sender, RepositorySaveEventArgs eventArgs) {
        //    IsolatedStorageHelper.SaveApplicationSetting("repository", _dataStore);
        //    UpdateUi();
        //}

        private void UpdateUi() {
            PopulateTags();
            PopulateAccounts();
        }

        private void PopulateTags() {
            Tags.Clear();
            var tagViewModels = _accounts.GetAllTags().Select(accountTag => new TagViewModel(accountTag));
            foreach (var tagViewModel in tagViewModels) {
                Tags.Add(tagViewModel);
            }
        }

        private void PopulateAccounts() {
            Accounts.Clear();
            var accountViewModels = _accounts.Select(account => new AccountViewModel(account));
            foreach (var accountViewModel in accountViewModels) {
                Accounts.Add(accountViewModel);
            }
        }

        public void Test(TagViewModel tag) {
            _navigationService.UriFor<TagsPivotPageViewModel>()
                .WithParam(x => x.ActiveTagName, tag.TagName)
                .Navigate();
        }

        public void ShowAccountDetails(AccountViewModel accountViewModel) {
            _navigationService.UriFor<AccountDetailsPageViewModel>()
                .WithParam(x => x.AccountId, accountViewModel.Id)
                .Navigate();
        }

        public void AddNewAccount() {
            _navigationService.UriFor<AccountProviderSelectPageViewModel>()
                .Navigate();
            ;
        }
    }

}
