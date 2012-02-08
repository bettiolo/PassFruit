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

        private ObservableCollection<LabelViewModel> _labels;

        public ObservableCollection<LabelViewModel> Labels {
            get {
                return _labels;
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
            PopulateLabels();
            PopulateAccounts();
        }

        private void PopulateLabels() {
            _labels = new ObservableCollection<LabelViewModel>();
            var labelViewModels = _repository.AccountLabels.GetAll().Select(accountLabel => {
                var label = new LabelViewModel();
                label.Id = accountLabel.Id;
                label.LabelName = accountLabel.Name;
                label.Description =
                    accountLabel.Description;
                return label;
            });

            foreach (var labelViewModel in labelViewModels) {
                Labels.Add(labelViewModel);
            }
        }

        private void PopulateAccounts() {
            _accounts = new ObservableCollection<AccountViewModel>();
            var accountViewModels = _repository.Accounts.GetAll().Select(account => {
                return new AccountViewModel(account);;
            });

            foreach (var accountViewModel in accountViewModels) {
                Accounts.Add(accountViewModel);
            }
        }

        public void Test(LabelViewModel label) {
            _navigationService.UriFor<LabelsPivotPageViewModel>()
                .WithParam(x => x.ActiveId, label.Id)
                .Navigate();
        }

        public void AddNewLabel() {
            var newlabel = "ciccio";
        }

        public void AddNewAccount() {
            var newaccount = "pluto";
        }
    }

}
