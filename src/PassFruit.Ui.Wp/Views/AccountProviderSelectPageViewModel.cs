using System;
using System.Collections.ObjectModel;
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
using PassFruit.DataStore.Contracts;
using PassFruit.Ui.Wp.Views.Controls;

namespace PassFruit.Ui.Wp.Views {

    public class AccountProviderSelectPageViewModel : Screen {

        private readonly INavigationService _navigationService;

        private IDataStore _dataStore;

        public AccountProviderSelectPageViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
            _dataStore = Init.GetDataStore();
            AccountProviders = new ObservableCollection<AccountProviderViewModel>();
            foreach (var provider in new Providers()) {
                AccountProviders.Add(new AccountProviderViewModel(provider));
            }
        }

        public ObservableCollection<AccountProviderViewModel> AccountProviders { get; private set; }


        public void AccountSelected(AccountProviderViewModel accountProvider) {

            
            //repository.Accounts.Add(provider.provider);

            //_navigationService.UriFor<AccountDetailsPageViewModel>()
            //    .WithParam(x => x.AccountId, )
            //    .Navigate();
        }

    }

}
