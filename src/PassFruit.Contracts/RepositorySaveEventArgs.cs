using System;

namespace PassFruit.Contracts {

    public class RepositorySaveEventArgs : EventArgs {
        
        public IRepository Repository { get; private set; }

        public IAccount Account { get; private set; }

        public RepositorySaveEventArgs(IRepository repository, IAccount account) {
            Repository = repository;
            Account = account;
        }

    }

}