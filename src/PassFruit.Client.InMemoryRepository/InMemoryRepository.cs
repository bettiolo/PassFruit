using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.InMemoryRepository {

    public class InMemoryRepository : RepositoryBase {

        public InMemoryRepository(InMemoryRepositoryConfiguration configuration) : base(configuration) {
            
        }

        public new InMemoryRepositoryConfiguration Configuration {
            get { return (InMemoryRepositoryConfiguration)base.Configuration; }
        }

        private readonly Dictionary<Guid, Dictionary<string, string>> _passwords = 
            new Dictionary<Guid, Dictionary<string, string>>();

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

        public override void DeletePasswords(Guid accountId) {
            _passwords.Remove(accountId);
        }

        protected override void InternalSave(IAccount account) {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Guid> GetAllAccountIds() {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Guid> GetDeletedAccountIds() {
            throw new NotImplementedException();
        }

        protected override IAccount LoadAccount(Guid accountId) {
            throw new NotImplementedException();
        }

        protected override void LoadAllFieldTypes() {
            throw new NotImplementedException();
        }

    }

}
