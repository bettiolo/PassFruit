using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class AccountTag : IAccountTag {

        private IRepository _repository;
        
        private Guid _id;

        public AccountTag(Guid id, IRepository repository) {
            _id = id;
            _repository = repository;
        }

        public Guid Id { get { return _id; } }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<IAccount> Accounts {
            get { return _repository.Accounts.Where(account => account.AccountTags.Contains(this)); }
        }

        public override string ToString() {
            return Name;
        }

    }
}
