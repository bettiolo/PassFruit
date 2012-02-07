using System;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace PassFruit.Ui.Wp {

    public class LabelsPivotPageViewModel : Conductor<LabelViewModel>.Collection.OneActive {

        private ObservableCollection<LabelViewModel> _labels;

        public ObservableCollection<LabelViewModel> Labels {
            get {
                if (_labels == null) {
                    PopulateLabels();
                }
                return _labels;
            }
        }

        public LabelViewModel ActiveLabel {
            get { return ActiveItem; }
            set { ActiveItem = value;
            NotifyOfPropertyChange(() => ActiveLabel);}
        }

        public Guid ActiveId {
            get { return ActiveItem.Id; }
            set {  }
        }

        LabelsPivotPageViewModel(Func<LabelsPivotPageViewModel> createCiccio ) {
            var uaz = createCiccio;
        }

        private void PopulateLabels() {
            IRepository repository;
            var app = Application.Current as App;
            if (app != null) {
                repository = app.Repositories.GetSelectedRepository();
            } else {
                var init = new Init();
                repository = init.GetRepositories().GetSelectedRepository();
            }


            _labels = new ObservableCollection<LabelViewModel>();
            var labelViewModels = repository.AccountLabels.GetAll().Select(accountLabel => {
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

            Items.AddRange(Labels);

        }

        public void Test() {
            var ciccio = DateTime.Now;
        }

    }

}
