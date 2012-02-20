using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IRepository {

        string Name { get; }

        string Description { get; }

        IAccounts Accounts { get; }

        IAccountTags AccountTags { get; }

        string GetPassword(Guid accountId);

        void SetPassword(Guid accountId, string password);

        event EventHandler<RepositorySaveEventArgs> OnSaved;

        event EventHandler<RepositorySaveEventArgs> OnSaving;

        void SaveAll();

        void Save(IAccount account);

    }

    public class RepositorySaveEventArgs : EventArgs {
        
        public IRepository Repository { get; private set; }

        public IAccount Account { get; private set; }

        public RepositorySaveEventArgs(IRepository repository, IAccount account) {
            Repository = repository;
            Account = account;
        }

    }
}
