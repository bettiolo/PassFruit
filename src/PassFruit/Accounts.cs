using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PassFruit.AccountImpl;
using PassFruit.Contracts;

namespace PassFruit {

    public class Accounts : Collection<IAccount>, IAccounts {

        private readonly IRepository _repository;

        internal Accounts(IRepository repository) {
            _repository = repository;
        }

        public IAccount this[Guid accountId] {
            get { return this.Single(account => account.Id == accountId); }
        }

        public IEnumerable<IAccount> GetByEmail(string email) {
            return this.Where(account => FindField(account, Contracts.FieldTypeKey.Email, email));
        }

        public IEnumerable<IAccount> GetByUserName(string userName) {
            return this.Where(account => FindField(account, Contracts.FieldTypeKey.UserName, userName));
        }

        public IAccount Create(string providerKey) {
            var provider = _repository.Providers.GetByKey(providerKey);
            return new Account(_repository, provider);
        }

        private static bool FindField(IAccount account, Contracts.FieldTypeKey fieldTypeKey, string fieldValue) {
            return account.GetFieldsByKey<string>(fieldTypeKey).Any(
                field => field.Value.Equals(fieldValue, StringComparison.OrdinalIgnoreCase)
            );
        }

        protected override void RemoveItem(int index) {
            var accountId = this[index].Id;
            this[index] = new DeletedAccount(_repository, accountId);
            _repository.Save(this[index]);
        } 

    }

}
