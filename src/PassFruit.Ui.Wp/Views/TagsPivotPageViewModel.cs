using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using PassFruit.Contracts;
using PassFruit.Ui.Wp.Views.Controls;

namespace PassFruit.Ui.Wp.Views {

    public class TagsPivotPageViewModel : Conductor<TagViewModel>.Collection.OneActive {

        private ObservableCollection<TagViewModel> _tags;

        public ObservableCollection<TagViewModel> Tags {
            get {
                if (_tags == null) {
                    PopulateTags();
                }
                return _tags;
            }
        }

        public TagViewModel ActiveTag {
            get { return ActiveItem; }
            set { ActiveItem = value;
            NotifyOfPropertyChange(() => ActiveTag);}
        }

        public Guid ActiveId {
            get { return ActiveItem.Id; }
            set { var ciccio = value; }
        }

        //TagsPivotPageViewModel(Func<TagsPivotPageViewModel> createCiccio ) {
        //    var uaz = createCiccio;
        //}

        public TagsPivotPageViewModel() {
            var test = "ciao";
        }


        //TagsPivotPageViewModel(Guid ActiveId) {
        //    var actie = ActiveId;
        //}

        private void PopulateTags() {
            var repository = Init.GetRepository();
            _tags = new ObservableCollection<TagViewModel>();
            var tagViewModels = repository.AccountTags.Select(accountTag => new TagViewModel(accountTag));
            foreach (var tagViewModel in tagViewModels) {
                Tags.Add(tagViewModel);
            }

            Items.AddRange(Tags);

        }

        public string Test {
            get { return DateTime.Now.ToShortDateString(); }
        }

        public void Test2() {
            var uaz = "asda";
        }

    }

}
