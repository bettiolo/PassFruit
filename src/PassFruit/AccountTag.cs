using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class AccountTag : IAccountTag {

        private readonly IRepository _repository;

        public AccountTag(IRepository repository) {
            _repository = repository;
        }

        public string Name { get; set; }

        public IEnumerable<IAccount> Accounts {
            get { return _repository.Accounts.Where(account => account.AccountTags.Contains(this)); }
        }

        public override string ToString() {
            return Name;
        }

        public bool Equals(AccountTag other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AccountTag)) return false;
            return Equals((AccountTag) obj);
        }

        public override int GetHashCode() {
            return (Name != null ? Name.GetHashCode() : 0);
        }

    }
}
