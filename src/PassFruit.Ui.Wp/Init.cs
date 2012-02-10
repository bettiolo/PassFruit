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
using Microsoft.Phone.Controls;
using PassFruit.Client;
using PassFruit.Client.FakeRepository;
using PassFruit.Contracts;
using PassFruit.Ui.Wp.Controls;

namespace PassFruit.Ui.Wp {

    public class Init {

       public Init() {
           TiltEffect.TiltableItems.Add(typeof(SettingsItem)); 
        }

        public Repositories GetRepositories() {
            var repositories = new Repositories();
            var fakeRepository = new FakeRepository();
            repositories.AddRepository(fakeRepository);
            repositories.SelectRepository(fakeRepository);
            return repositories;
        }

        public static IRepository GetRepository() {
            IRepository repository;
            var app = Application.Current as App;
            if (app != null) {
                repository = app.Repositories.GetSelectedRepository();
            } else {
                var init = new Init();
                repository = init.GetRepositories().GetSelectedRepository();
            }
            return repository;
        }
        
    }

}
