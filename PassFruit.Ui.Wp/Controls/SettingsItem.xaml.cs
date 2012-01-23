using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PassFruit.Ui.Wp.Controls {

    public partial class SettingsItem : UserControl, INotifyPropertyChanged {

        //private string _title;

        public SettingsItem() {
            InitializeComponent();
            Title = "TEST!";
        }

        #region Title

        public string Title {
            get { return GetValue(TitleProperty).ToString(); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
              DependencyProperty.Register("Title", typeof(string), typeof(SettingsItem),
              new PropertyMetadata(string.Empty, OnTitlePropertyChanged));

        private static void OnTitlePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            var settingsItem = dependencyObject as SettingsItem;
            settingsItem.OnPropertyChanged("Title");
        }

        #endregion

        #region Description

        public string Description {
            get { return GetValue(DescriptionProperty).ToString(); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
              DependencyProperty.Register("Description", typeof(string), typeof(SettingsItem),
              new PropertyMetadata(string.Empty, OnDescriptionPropertyChanged));

        private static void OnDescriptionPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            var settingsItem = dependencyObject as SettingsItem;
            settingsItem.OnPropertyChanged("Description");
        }
        
        #endregion

        #region PageUrl

        public string PageUrl {
            get { return GetValue(PageUrlProperty).ToString(); }
            set { SetValue(PageUrlProperty, value); }
        }

        public static readonly DependencyProperty PageUrlProperty =
              DependencyProperty.Register("PageUrl", typeof(string), typeof(SettingsItem),
              new PropertyMetadata(string.Empty, OnPageUrlPropertyChanged));

        private static void OnPageUrlPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            var settingsItem = dependencyObject as SettingsItem;
            settingsItem.OnPropertyChanged("PageUrl");
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            var root = Application.Current.RootVisual as PhoneApplicationFrame;
            if (root != null) {
                root.Navigate(new Uri(PageUrl, UriKind.RelativeOrAbsolute));
            }
        }

    }
}
