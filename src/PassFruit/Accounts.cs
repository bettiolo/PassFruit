using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PassFruit.AccountImplementations;
using PassFruit.Contracts;

namespace PassFruit {

    public class Accounts : Collection<IAccount>, IAccounts {

        private readonly IRepository _repository;

        public Accounts(IRepository repository) : base() {
            _repository = repository;
        }

        public IAccount this[Guid accountId] {
            get { return this.Single(account => account.Id == accountId); }
        }

        public IEnumerable<IAccountWithEmail> GetByEmail(string email) {
            return this.OfType<IAccountWithEmail>().Where(
                account => account.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
            );
        }

        public IEnumerable<IAccountWithUserName> GetByUserName(string userName) {
            return this.OfType<IAccountWithUserName>().Where(
                account => account.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
            );
        }

        protected override void RemoveItem(int index) {
            var accountId = this[index].Id;
            this[index] = new DeletedAccount(_repository, accountId);
            _repository.Save(this[index]);
        } 

    }

}
