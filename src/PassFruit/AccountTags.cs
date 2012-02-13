using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PassFruit.Contracts;

namespace PassFruit {

    public class AccountTags : Collection<IAccountTag>, IAccountTags {

        private readonly IRepository _repository;

        public AccountTags(IRepository repository) {
            _repository = repository;
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public IAccountTag this[Guid id] {
            get {
                return this.FirstOrDefault(accountTag =>
                    accountTag.Id == id);
            }
        }

        public IAccountTag this[string name] {
            get {
                return this.FirstOrDefault(accountTag => 
                    accountTag.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        public IEnumerable<IAccountTag> GetByAccountId(Guid accountId) {
            return this.Where(accountTag => accountTag.Accounts.Any(account => account.Id == accountId));
        }



    }

}
