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
using PassFruit.Client;
using PassFruit.Client.FakeRepository;
using PassFruit.Contracts;

namespace PassFruit.Ui.Wp {

    public class Init {

        private IRepository _repository;
        
        public Init() {
            var repositories = new Repositories();
            var fakeRepository = new FakeRepository();
            repositories.AddRepository(fakeRepository);
            repositories.SelectRepository(fakeRepository);
            var repository = repositories.GetSelectedRepository();
            var groups = repository.AccountGroups.GetAll();
            var accounts = groups[0].Accounts;
        }
        
    }

}
