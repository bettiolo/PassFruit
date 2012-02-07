using System;
using Caliburn.Micro;

namespace PassFruit.Ui.Wp {

    public class LabelViewModel : PropertyChangedBase {

        public LabelViewModel() {

        }

        private Guid _id;
        public Guid Id {
            get { return _id; }
            set {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        private string _labelName;
        public string LabelName {
            get { return _labelName; }
            set {
                _labelName = value;
                NotifyOfPropertyChange(() => LabelName);
            }
        }

        private string _description;
        public string Description {
            get { return _description; }
            set {
                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }
    }
}
