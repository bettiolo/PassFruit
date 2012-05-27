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

    public class PasswordFieldViewModel : AccountFieldBase {

        private readonly IAccount _account;

        public PasswordFieldViewModel(IAccount account) {
            _account = account;
            FieldName = "Password";
            // FieldValue = field.Value;
            CopyButtonText = "copy " + FieldName;
            ShowButtonText = "shows " + FieldName;
        }

        private string _fieldName;
        public string FieldName {
            get { return _fieldName; }
            set {
                _fieldName = value;
                NotifyOfPropertyChange(() => _fieldName);
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


        private string _showButtonText;
        public string ShowButtonText {
            get { return _showButtonText; }
            set {
                _showButtonText = value;
                NotifyOfPropertyChange(() => ShowButtonText);
            }
        }

        public void CopyPassword() {
            Clipboard.SetText(_account.GetPassword());
            MessageBox.Show("Password copied");
        }

        public void ShowPassword() {
            MessageBox.Show("Password is '" + _account.GetPassword() + "'");
        }

    }

}
