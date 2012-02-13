using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using PassFruit.Contracts;
using PassFruit.Ui.Wp.Views;
using PassFruit.Ui.Wp.Views.Controls;

namespace PassFruit.Ui.Wp {

    public class MainPageViewModel : Screen {

        private readonly IRepository _repository;

        INavigationService _navigationService;

        private ObservableCollection<TagViewModel> _tags;

        public ObservableCollection<TagViewModel> Tags {
            get {
                return _tags;
            }
        }

        private ObservableCollection<AccountViewModel> _accounts;

        public ObservableCollection<AccountViewModel> Accounts {
            get {
                return _accounts;
            }
        }

        public MainPageViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
            var app = Application.Current as App;
            if (app != null) {
                _repository = app.Repositories.GetSelectedRepository();
            } else {
                var init = new Init();
                _repository = init.GetRepositories().GetSelectedRepository();
            }

            var fakeData = new FakeDataGenerator();
            fakeData.GenerateFakeData(_repository);

            PopulateTags();
            PopulateAccounts();
        }

        private void PopulateTags() {
            _tags = new ObservableCollection<TagViewModel>();
            var tagViewModels = _repository.AccountTags.Select(accountTag => new TagViewModel(accountTag));
            foreach (var tagViewModel in tagViewModels) {
                Tags.Add(tagViewModel);
            }
        }

        private void PopulateAccounts() {
            _accounts = new ObservableCollection<AccountViewModel>();
            var accountViewModels = _repository.Accounts.Select(account => {
                return new AccountViewModel(account);;
            });

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
            var newaccount = "pluto";
        }
    }

}
