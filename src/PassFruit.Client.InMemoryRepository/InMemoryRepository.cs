using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.InMemoryRepository {

    public class InMemoryRepository : RepositoryBase {

        private Dictionary<Guid, string> _passwords = new Dictionary<Guid, string>(); 

        public override string Name {
            get { return "In Memory Repository"; }
        }

        public override string Description {
            get { return "In Memory repository, the data is not persisted anywhere"; }
        }

        public override string GetPassword(Guid accountId) {
            return _passwords[accountId];
        }

        public override void SetPassword(Guid accountId, string password) {
            _passwords[accountId] = password;
        }

        protected override IAccounts GetAllAccounts() {
            return new Accounts(this);
        }

        protected override IAccountTags GetAllAccountTags() {
            return new AccountTags(this);
        }

        protected override IAccounts GetAllAccountsExceptDeleted() {
            return new Accounts(this);
        }

    }

}
