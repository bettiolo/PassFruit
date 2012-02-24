using System;
using System.ComponentModel;
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

namespace PassFruit.Ui.Wp.Views.Controls {

    public class AccountProviderIconViewModel : PropertyChangedBase {

        public AccountProviderIconViewModel(IProvider provider, int size) {
            var basePath = ImageSource = @"/Resources/Providers/" + size + "/";
            if (provider == null) {
                 ImageSource = basePath + "Unknown.png";
            } else {
                ImageSource = basePath + provider.Name + ".png";
            }
        }

        private string _imageSource;
        public string ImageSource { 
            get {
                return _imageSource; 
            } 
            set { 
                _imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

    }
    
}
