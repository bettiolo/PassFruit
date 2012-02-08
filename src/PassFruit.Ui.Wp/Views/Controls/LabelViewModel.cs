using System;
using Caliburn.Micro;

namespace PassFruit.Ui.Wp.Views.Controls {

    public class LabelViewModel : PropertyChangedBase {

        private Guid _id;
        public Guid Id {
            get { return _id; }
            set {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        private string _labelName;
        public virtual string LabelName {
            get { return _labelName; }
            set {
                _labelName = value;
                NotifyOfPropertyChange(() => LabelName);
            }
        }

        private string _description;
        public virtual string Description {
            get { return _description; }
            set {
                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }
    }

    public class SampleLabelViewModel : LabelViewModel {
        
        public override string LabelName {
            get { return "Label name"; }
        }

        public override string Description {
            get {
                return "Label description bla bla bla bla bla bla bla bla";
            }
            set {
                base.Description = value;
            }
        }

    }
}
