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
            if (OnSaving != null) {
                OnSaving(this, EventArgs.Empty);
            }
            account.SetClean();
            if (OnSaved != null) {
                OnSaved(this, EventArgs.Empty);
            }
        }

        public event EventHandler OnSaved;

        public event EventHandler OnSaving;

        public void SaveAll() {
            if (OnSaving != null) {
                OnSaving(this, EventArgs.Empty);
            }
            foreach (var account in Accounts.Where(a => a.IsDirty)) {
                account.SetClean();   
            }
            if (OnSaved != null) {
                OnSaved(this, EventArgs.Empty);
            }
        }

        protected abstract IAccounts GetAllAccounts();

        protected abstract IAccountTags GetAllAccountTags();

        protected abstract IAccounts GetAllAccountsExceptDeleted();

    }

}
