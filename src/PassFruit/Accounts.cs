using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PassFruit.Contracts;
using PassFruit.DataStore.Contracts;

namespace PassFruit {

    public class Accounts : IAccounts {

        private readonly IDataStore _dataStore;

        private readonly IPasswords _passwords;

        private readonly List<IAccount> _accounts = new List<IAccount>();

        private readonly Providers _providers;

        private FieldTypes _fieldTypes;

        public Accounts(IDataStore dataStore, IPasswords passwords) {
            _dataStore = dataStore;
            _passwords = passwords;
            _providers = new Providers();
            _fieldTypes = new FieldTypes();
            _accounts.AddRange(dataStore.GetActiveAccountDtos().Select(accountDto => new Account(accountDto)));
        }

        public IAccount this[Guid accountId] {
            get { return this.Single(account => account.Id == accountId); }
        }

        public IAccount GetById(Guid accountId) {
            return this.SingleOrDefault(account => account.Id == accountId);
        }

        public IEnumerable<IAccount> GetByEmail(string email) {
            return this.Where(account => FindField(account, FieldTypeKey.Email, email));
        }

        public IEnumerable<IAccount> GetByUserName(string userName) {
            return this.Where(account => FindField(account, FieldTypeKey.UserName, userName));
        }

        public IEnumerable<IAccount> GetByTag(string tagKey) {
            return _accounts.Where(account => account.Tags.Contains(tagKey));
        }

        public IAccount Create(string providerKey, Guid? id = null) {
            var provider = _providers.GetByKey(providerKey);
            return new Account(_passwords, provider, _fieldTypes, DateTime.UtcNow, id);
        }

        private static bool FindField(IAccount account, FieldTypeKey fieldTypeKey, string fieldValue) {
            return account.GetFieldsByKey(fieldTypeKey).Any(
                field => field.Value.ToString().Equals(fieldValue, StringComparison.OrdinalIgnoreCase)
            );
        }

        public void Remove(Guid accountId) {
            var account = GetById(accountId);
            if (account is DeletedAccount) {
                if (!account.IsDirty) {
                    _accounts.Remove(account);
                }
            } else {
                account.DeleteAllPasswords();
                _accounts.Remove(account);
                _accounts.Add(new DeletedAccount(_passwords, accountId));
            }
        }

        public IEnumerator<IAccount> GetEnumerator() {
            return _accounts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

         
    }

}
