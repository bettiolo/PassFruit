using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.InMemoryRepository {

    public class InMemoryRepository : RepositoryBase {

        private readonly Dictionary<Guid, Dictionary<string, string>> _passwords = new Dictionary<Guid, Dictionary<string, string>>();

        public override string Name {
            get { return "In Memory Repository"; }
        }

        public override string Description {
            get { return "In Memory repository, the data is not persisted anywhere"; }
        }

        public override string GetPassword(Guid accountId, string passwordKey = DefaultPasswordKey) {
            if (String.IsNullOrEmpty(passwordKey)) {
                passwordKey = DefaultPasswordKey;
            }
            passwordKey = passwordKey.ToLowerInvariant();
            var accountPasswords = _passwords[accountId];
            if (accountPasswords == null || !accountPasswords.ContainsKey(passwordKey)) {
                return "";
            }
            return accountPasswords[passwordKey];
        }

        public override void SetPassword(Guid accountId, string password, string passwordKey = DefaultPasswordKey) {
            if (String.IsNullOrEmpty(passwordKey)) {
                passwordKey = DefaultPasswordKey;
            }
            passwordKey = passwordKey.ToLowerInvariant();
            if (!_passwords.ContainsKey(accountId)) {
                _passwords.Add(accountId, new Dictionary<string, string>());
            }
            if (!_passwords[accountId].ContainsKey(passwordKey)) {
                _passwords[accountId].Add(passwordKey, "");
            }
            _passwords[accountId][passwordKey] = password;
        }

        protected override void LoadAllAccounts() {
            // Accounts.Add(Accounts.Create());
        }

        protected override void LoadAllAccountsExceptDeleted() {
            // Accounts.Add(Accounts.Create());
        }

        protected override void LoadAllFieldTypes() {
            // FieldTypes.Add(FieldTypes.Create());
        }
    }

}
