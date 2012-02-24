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

        protected override void LoadAllAccounts() {
            // Accounts.Add(Accounts.Create());
        }

        protected override void LoadAllAccountsExceptDeleted() {
            // Accounts.Add(Accounts.Create());
        }

        protected override void LoadAllAccountProviders() {
            // Providers.Add(Providers.Create());
        }

        protected override void LoadAllFieldTypes() {
            // FieldTypes.Add(FieldTypes.Create());
        }
    }

}
