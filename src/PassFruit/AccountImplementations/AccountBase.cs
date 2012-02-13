using System;
using System.Collections.Generic;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public abstract class AccountBase : IAccount {
        
        private readonly IRepository _repository;

        public AccountBase(IRepository repository) {
            _repository = repository;
            AccountTags = new List<IAccountTag>();
        }

        public Guid Id { get; set; }

        public abstract string AccountName { get; }

        public string Notes { get; set; }

        public override string ToString() {
            return AccountName + " | " + Provider.Name;
        }

        public abstract IAccountProvider Provider { get; }

        public string GetPassword() {
            return _repository.GetPassword(Id);
        }

        public void SetPassword(string password) {
            _repository.SetPassword(Id, password);
        }

        public IList<IAccountTag> AccountTags { get; private set; }

        public void AddTag(string tagName) {
            AccountTags.Add(_repository.AccountTags[tagName]);
        }

        public virtual void Save() {
            // Save data
        }
    }

}


