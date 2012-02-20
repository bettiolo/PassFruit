using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PassFruit.Contracts;

namespace PassFruit {

    public class AccountTags : IAccountTags {

        private readonly IRepository _repository;

        private readonly List<IAccountTag> _accountTags = new List<IAccountTag>();

        public AccountTags(IRepository repository) {
            _repository = repository;
            ReloadTags();
        }

        private void ReloadTags() {
            _accountTags.Clear();
            _accountTags.AddRange(_repository.Accounts.SelectMany(account => account.AccountTags).Distinct());
        }

        public IAccountTag this[string name] {
            get {
                return this.FirstOrDefault(accountTag => 
                    accountTag.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool Contains(string name) {
            return _accountTags.Any(accountTag => accountTag.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<IAccountTag> GetByAccountId(Guid accountId) {
            return this.Where(accountTag => accountTag.Accounts.Any(account => account.Id == accountId));
        }

        public IEnumerator<IAccountTag> GetEnumerator() {
            return _accountTags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

}
