using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp.Views.Controls.Fields {

    public class NotesFieldViewModel : AccountFieldBase {

        public NotesFieldViewModel(IAccount account) {
            _account = account;
            _notes = _account.Notes;
        }

        private string _notes;

        private IAccount _account;

        public string Notes {
            get { return _notes; }
            set {
                _notes = value;
                NotifyOfPropertyChange(() => _notes);
            }
        }

    }

}
