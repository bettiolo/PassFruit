using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using PassFruit.Contracts;

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

        public void Test(LabelViewModel label) {
            _navigationService.UriFor<LabelsPivotPageViewModel>()
                .Navigate();
        }

    }

}
