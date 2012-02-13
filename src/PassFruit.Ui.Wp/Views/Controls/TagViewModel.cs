using System;
using Caliburn.Micro;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp.Views.Controls {

    public class TagViewModel : PropertyChangedBase {

        public TagViewModel(IAccountTag accountTag) {
            Id = accountTag.Id;
            TagName = accountTag.Name;
            Description = accountTag.Description;
        }

        private Guid _id;
        public Guid Id {
            get { return _id; }
            set {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        private string _tagName;
        public virtual string TagName {
            get { return _tagName; }
            set {
                _tagName = value;
                NotifyOfPropertyChange(() => TagName);
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

}
