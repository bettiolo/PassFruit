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

        private readonly List<IAccount> _accounts = new List<IAccount>();

        private readonly Providers _providers;

        private FieldTypes _fieldTypes;

        public Accounts(IDataStore dataStore) {
            _dataStore = dataStore;
            _providers = new Providers();
            _fieldTypes = new FieldTypes();
            foreach (var accountDto in dataStore.GetAccountDtos()) {
                var account = new Account(accountDto, _dataStore.GetPasswordDtos(accountDto.Id), _fieldTypes);
                _accounts.Add(account);
            }
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
            // return new Account(_passwords, provider, _fieldTypes, DateTime.UtcNow, id);
            throw new NotSupportedException();
        }

        private static bool FindField(IAccount account, FieldTypeKey fieldTypeKey, string fieldValue) {
            return account.GetFieldsByKey(fieldTypeKey).Any(
                field => field.Value.ToString().Equals(fieldValue, StringComparison.OrdinalIgnoreCase)
            );
        }

        public void Remove(Guid accountId) {
            //var account = GetById(accountId);
            //if (account is DeletedAccount) {
            //    if (!account.IsDirty) {
            //        _accounts.Remove(account);
            //    }
            //} else {
            //    account.DeleteAllPasswords();
            //    _accounts.Remove(account);
            //    // _accounts.Add(new DeletedAccount(_passwords, accountId));
            //}
            throw new NotSupportedException();
        }

        public IEnumerator<IAccount> GetEnumerator() {
            return _accounts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerable<ITag> GetAllTags() {
            IList<ITag> tags = new List<ITag>();
            foreach (var tag in _accounts.SelectMany(account => account.Tags.Where(tag => !tags.Contains(tag)))) {
                tags.Add(tag);
            }
            return tags;
        }

    }

}
