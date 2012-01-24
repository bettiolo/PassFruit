using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using PassFruit.Ui.Wp.Controls;

namespace PassFruit.Ui.Wp {
    public partial class MainPage : PhoneApplicationPage {

        public MainPage() {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(SettingsItem)); 
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) {

        }
    }
}