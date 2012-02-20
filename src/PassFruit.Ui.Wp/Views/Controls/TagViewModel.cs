using System;
using System.Linq;
using Caliburn.Micro;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp.Views.Controls {

    public class TagViewModel : PropertyChangedBase {

        public TagViewModel(IAccountTag accountTag) {
            TagName = accountTag.Name;
            Description = accountTag.Accounts.Count() + " account(s)";
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
