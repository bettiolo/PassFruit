using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using IsolatedStorageExtensions;
using PassFruit.Contracts;
using PassFruit.Tests.FakeData;
using PassFruit.Ui.Wp.Views;
using PassFruit.Ui.Wp.Views.Controls;

namespace PassFruit.Ui.Wp {

    public class MainPageViewModel : Screen {

        private readonly IRepository _repository;

        INavigationService _navigationService;

        public ObservableCollection<TagViewModel> Tags { get; private set; }

        public ObservableCollection<AccountViewModel> Accounts { get; private set; }

        public MainPageViewModel(INavigationService navigationService) {
            Tags = new ObservableCollection<TagViewModel>();
            Accounts = new ObservableCollection<AccountViewModel>();
            _navigationService = navigationService;

            var storedRepository = IsolatedStorageHelper.GetApplicationSetting("repository") as IRepository;
            if (storedRepository == null) {
                var app = Application.Current as App;
                if (app != null) {
                    _repository = app.Repositories.GetSelectedRepository();
                } else {
                    var init = new Init();
                    _repository = init.GetRepositories().GetSelectedRepository();
                }
                var fakeData = new FakeDataGenerator();
                fakeData.GenerateFakeData(_repository);
            } else {
                _repository = storedRepository;
            }

            UpdateUi();

            _repository.OnSaved += RepositoryChanged;

        }

        private void RepositoryChanged(object sender, EventArgs eventArgs) {
            IsolatedStorageHelper.SaveApplicationSetting("repository", _repository);
            UpdateUi();
        }

        private void UpdateUi() {
            PopulateTags();
            PopulateAccounts();
        }

        private void PopulateTags() {
            Tags.Clear();
            var tagViewModels = _repository.AccountTags.Select(accountTag => new TagViewModel(accountTag));
            foreach (var tagViewModel in tagViewModels) {
                Tags.Add(tagViewModel);
            }
        }

        private void PopulateAccounts() {
            Accounts.Clear();
            var accountViewModels = _repository.Accounts.Select(account => new AccountViewModel(account));
            foreach (var accountViewModel in accountViewModels) {
                Accounts.Add(accountViewModel);
            }
        }

        public void Test(TagViewModel tag) {
            _navigationService.UriFor<TagsPivotPageViewModel>()
                .WithParam(x => x.ActiveId, tag.Id)
                .Navigate();
        }

        public void ShowAccountDetails(AccountViewModel accountViewModel) {
            _navigationService.UriFor<AccountDetailsPageViewModel>()
                .WithParam(x => x.AccountId, accountViewModel.Id)
                .Navigate();
        }

        public void AddNewTag() {
            var newtag = "ciccio";
        }

        public void AddNewAccount() {
            _navigationService.UriFor<AccountProviderSelectPageViewModel>()
                .Navigate();
            ;
        }
    }

}
