using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PassFruit.Client;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp.ViewModels {

    public class MainPageViewModel : INotifyPropertyChanged {

        private IRepository _repository;

        private ObservableCollection<AccountGroupViewModel> _groups;

        public ObservableCollection<AccountGroupViewModel> Groups { 
            get {
                if (_groups == null) {
                    PopulateGroups();
                }
                return _groups;

            }
        }

        static MainPageViewModel() {
            
        }
        
        public MainPageViewModel() {
            var app = Application.Current as App;
            if (app != null) {
                _repository = app.Repositories.GetSelectedRepository();
            } else {
                var init = new Init();
                _repository = init.GetRepositories().GetSelectedRepository();
            }

            //_groups = _repository.AccountGroups.GetAll().Select(accountGroup =>
            //    new AccountGroupViewModel {
            //        Id = accountGroup.Id,
            //        Name = accountGroup.Name,
            //        Description = accountGroup.Description
            //    }
            //).ToList();
            //OnPropertyChanged("Groups");
        }

        //public IList<AccountGroupViewModel> Groups {
        //    get {
        //        return _groups;
        //    }
        //}

        private void PopulateGroups() {

            _groups = new ObservableCollection<AccountGroupViewModel>();
            var groupViewModels = _repository.AccountGroups.GetAll().Select(accountGroup =>
                new AccountGroupViewModel {
                    Id = accountGroup.Id,
                    Name = accountGroup.Name,
                    Description = accountGroup.Description
                }
            );

            foreach (var groupViewModel in groupViewModels) {
                Groups.Add(groupViewModel);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
