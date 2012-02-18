using System;
using System.Collections.Generic;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public abstract class AccountBase : IAccount {
        
        private readonly IRepository _repository;

        private int _orignalHash;

        public AccountBase(IRepository repository, Guid? id = null) {
            _repository = repository;
            AccountTags = new List<IAccountTag>();
            Id = id.HasValue ? id.Value : Guid.NewGuid();
        }

        public Guid Id { get; protected set; }

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
            if (IsDirty) {
                _repository.Save(this);
            }
        }

        public bool IsDirty {
            get { return (_orignalHash != GetHashCode()); }
        }

        public override int GetHashCode() {
            unchecked {
                int result = Id.GetHashCode();
                result = (result*397) ^ (Notes != null ? Notes.GetHashCode() : 0);
                result = (result*397) ^ (AccountTags != null ? AccountTags.GetHashCode() : 0);
                return result;
            }
        }

        public bool Equals(AccountBase other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id) && Equals(other.Notes, Notes) && Equals(other.AccountTags, AccountTags);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AccountBase)) return false;
            return Equals((AccountBase) obj);
        }

        public void SetClean() {
            _orignalHash = GetHashCode();
        }

    }

}


