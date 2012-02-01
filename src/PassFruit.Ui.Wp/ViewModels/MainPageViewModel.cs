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

        private readonly IRepository _repository;

        private ObservableCollection<AccountTagViewModel> _tags;

        public ObservableCollection<AccountTagViewModel> Tags { 
            get {
                if (_tags == null) {
                    PopulateTags();
                }
                return _tags;

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

            //_tags = _repository.AccountTags.GetAll().Select(accountTag =>
            //    new AccountTagViewModel {
            //        Id = accountTag.Id,
            //        Name = accountTag.Name,
            //        Description = accountTag.Description
            //    }
            //).ToList();
            //OnPropertyChanged("Tags");
        }

        //public IList<AccountTagViewModel> Tags {
        //    get {
        //        return _tags;
        //    }
        //}

        private void PopulateTags() {

            _tags = new ObservableCollection<AccountTagViewModel>();
            var tagViewModels = _repository.AccountTags.GetAll().Select(accountTag =>
                new AccountTagViewModel {
                    Id = accountTag.Id,
                    Name = accountTag.Name,
                    Description = accountTag.Description
                }
            );

            foreach (var tagViewModel in tagViewModels) {
                Tags.Add(tagViewModel);
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
