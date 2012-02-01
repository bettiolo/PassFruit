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

        private ObservableCollection<AccountLabelViewModel> _labels;

        public ObservableCollection<AccountLabelViewModel> Labels { 
            get {
                if (_labels == null) {
                    PopulateLabels();
                }
                return _labels;

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

            //_labels = _repository.AccountLabels.GetAll().Select(accountLabel =>
            //    new AccountLabelViewModel {
            //        Id = accountLabel.Id,
            //        Name = accountLabel.Name,
            //        Description = accountLabel.Description
            //    }
            //).ToList();
            //OnPropertyChanged("Labels");
        }

        //public IList<AccountLabelViewModel> Labels {
        //    get {
        //        return _labels;
        //    }
        //}

        private void PopulateLabels() {

            _labels = new ObservableCollection<AccountLabelViewModel>();
            var labelViewModels = _repository.AccountLabels.GetAll().Select(accountLabel =>
                new AccountLabelViewModel {
                    Id = accountLabel.Id,
                    Name = accountLabel.Name,
                    Description = accountLabel.Description
                }
            );

            foreach (var labelViewModel in labelViewModels) {
                Labels.Add(labelViewModel);
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
