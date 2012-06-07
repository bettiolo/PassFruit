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
using Caliburn.Micro;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp.Views.Controls.Fields {

    public class GenericAccountFieldViewModel : AccountFieldBase {

        public GenericAccountFieldViewModel(IField field) {
            FieldName = field.Name;
            FieldValue = field.Value as string;
            CopyButtonText = "copy " + FieldName;
            IsCopyButtonVisible = true;
        }

        private string _fieldName;
        public string FieldName {
            get { return _fieldName; }
            set {
                _fieldName = value;
                NotifyOfPropertyChange(() => _fieldName);
            }
        }

        private string _fieldValue;
        public string FieldValue {
            get { return _fieldValue; }
            set {
                _fieldValue = value;
                NotifyOfPropertyChange(() => FieldValue);
            }
        }

        private string _copyButtonText;
        public string CopyButtonText {
            get { return _copyButtonText; }
            set {
                _copyButtonText = value;
                NotifyOfPropertyChange(() => CopyButtonText);
            }
        }

        private bool _isCopyButtonVisible;
        public bool IsCopyButtonVisible {
            get { return _isCopyButtonVisible; }
            set {
                _isCopyButtonVisible = value;
                NotifyOfPropertyChange(() => IsCopyButtonVisible);
            }
        }

        public void Copy() {
            Clipboard.SetText(FieldValue);
            MessageBox.Show(FieldName + " copied: '" + FieldValue + "'");
        }

    }

}
