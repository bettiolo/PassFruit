using System;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {
    
    public abstract class RepositoryBase : IRepository {

        private IAccounts _accounts;

        private ITags _tags;

        private IProviders _providers;

        private IFieldTypes _fieldTypes;

        public abstract string Name { get; }

        public abstract string Description { get; }
        
        public IAccounts Accounts {
            get {
                if (_accounts == null) {
                    _accounts = new Accounts(this);
                    LoadAllAccountsExceptDeleted();
                }
                return _accounts;
            }
        }

        public ITags Tags {
            get { return _tags ?? (_tags = new Tags(this)); }
        }

        public IProviders Providers {
            get {
                if (_providers == null) {
                    _providers = new Providers(this);
                    LoadAllAccountProviders();
                }
                return _providers;
            }
        }

        public IFieldTypes FieldTypes {
            get {
                if (_fieldTypes == null) {
                    _fieldTypes = new FieldTypes(this);
                    LoadAllFieldTypes();
                }
                return _fieldTypes;
            }
        }

        public abstract string GetPassword(Guid accountId);

        public abstract void SetPassword(Guid accountId, string password);

        public void Save(IAccount account) {
            _tags = null;
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

        protected abstract void LoadAllAccounts();

        protected abstract void LoadAllAccountsExceptDeleted();

        protected abstract void LoadAllAccountProviders();

        protected abstract void LoadAllFieldTypes();

    }
}
