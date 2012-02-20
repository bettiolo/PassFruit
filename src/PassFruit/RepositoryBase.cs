using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.AccountImplementations;
using PassFruit.Contracts;

namespace PassFruit {
    
    public abstract class RepositoryBase : IRepository {
        
        private IAccounts _accounts;

        private IAccountTags _accountTags;

        protected RepositoryBase() {
            
        }
        
        public abstract string Name { get; }

        public abstract string Description { get; }
        
        public IAccounts Accounts {
            get { return _accounts ?? (_accounts = GetAllAccountsExceptDeleted()); }
        }

        public IAccountTags AccountTags {
            get { return _accountTags ?? (_accountTags = GetAllAccountTags()); }
        }

        public abstract string GetPassword(Guid accountId);

        public abstract void SetPassword(Guid accountId, string password);

        public void Save(IAccount account) {
            _accountTags = null;
            if (OnSaving != null) {
                OnSaving(this, new RepositorySaveEventArgs(this, account));
            }
            account.SetClean();
            if (OnSaved != null) {
                OnSaved(this, new RepositorySaveEventArgs(this, account));
            }
        }

        public event EventHandler<RepositorySaveEventArgs> OnSaved;

        public event EventHandler<RepositorySaveEventArgs> OnSaving;

        public void SaveAll() {
            foreach (var account in Accounts.Where(a => a.IsDirty)) {
                account.Save();
            }
        }

        protected abstract IAccounts GetAllAccounts();

        protected abstract IAccountTags GetAllAccountTags();

        protected abstract IAccounts GetAllAccountsExceptDeleted();

    }

}
