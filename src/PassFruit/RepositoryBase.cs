using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {
    
    public abstract class RepositoryBase : IRepository {

        public const string DefaultPasswordKey = "default";

        public RepositoryBase(IRepositoryConfiguration configuration) {
            Configuration = configuration;
        }

        protected IRepositoryConfiguration Configuration { get; set; }

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
                    foreach (var account in GetAllAccountIds().Select(LoadAccount).Where(account => account != null)) {
                        _accounts.Add(account);
                    }
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

        public abstract string GetPassword(Guid accountId, string passwordKey = DefaultPasswordKey);

        public abstract void SetPassword(Guid accountId, string password, string passwordKey = DefaultPasswordKey);

        public void Save(IAccount account) {
            _tags = null;
            if (OnSaving != null) {
                OnSaving(this, new RepositorySaveEventArgs(this, account));
            }
            InternalSave(account);
            account.SetClean();
            if (OnSaved != null) {
                OnSaved(this, new RepositorySaveEventArgs(this, account));
            }
        }

        public bool IsDirty() {
            return Accounts.Any(account => account.IsDirty);
        }

        public abstract void DeletePasswords(Guid accountId);

        protected abstract void InternalSave(IAccount account);

        public event EventHandler<RepositorySaveEventArgs> OnSaved;

        public event EventHandler<RepositorySaveEventArgs> OnSaving;

        public void SaveAll() {
            var deletedAccounts = new List<DeletedAccount>();
            foreach (var account in Accounts.Where(a => a.IsDirty)) {
                account.Save();
                var deletedAccount = account as DeletedAccount;
                if (deletedAccount != null) {
                    deletedAccounts.Add(deletedAccount);
                }
            }
            foreach (var deletedAccount in deletedAccounts) {
                Accounts.Remove(deletedAccount);
            }
        }

        protected abstract IEnumerable<Guid> GetAllAccountIds(bool includingDeleted = false);

        protected abstract IAccount LoadAccount(Guid accountId);

        protected void LoadAllAccountProviders() {
            Providers.Add("generic", "Generic", true, true, false, "");
            Providers.Add("facebook", "Facebook", true, false, false, "");
            Providers.Add("twitter", "Twitter", true, true, false, "");
            Providers.Add("google", "Google", true, false, false, "");
        }

        protected abstract void LoadAllFieldTypes();

    }
}
