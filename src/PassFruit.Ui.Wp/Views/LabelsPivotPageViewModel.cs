using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using PassFruit.Contracts;
using PassFruit.Ui.Wp.Views.Controls;

namespace PassFruit.Ui.Wp.Views {

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
            set { var ciccio = value; }
        }

        //LabelsPivotPageViewModel(Func<LabelsPivotPageViewModel> createCiccio ) {
        //    var uaz = createCiccio;
        //}

        public LabelsPivotPageViewModel() {
            var test = "ciao";
        }


        //LabelsPivotPageViewModel(Guid ActiveId) {
        //    var actie = ActiveId;
        //}

        private void PopulateLabels() {
            var repository = Init.GetRepository();
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

        public string Test {
            get { return DateTime.Now.ToShortDateString(); }
        }

        public void Test2() {
            var uaz = "asda";
        }

    }

}
