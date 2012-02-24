using System;
using System.Collections.Generic;
using System.Linq;
using PassFruit.Contracts;

namespace PassFruit {

    public class Account : IAccount {

        private readonly IRepository _repository;

        private int _orignalHash;

        internal Account(IRepository repository, Guid? id = null) {
            _repository = repository;
            Tags = new List<ITag>();
            Fields = new List<IField>();
            Id = id.HasValue ? id.Value : Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public string GetAccountName() {
            var accountName = "";
            if (Provider.HasUserName) {
                var userNameField = GetDefaultField(FieldTypeName.UserName);
                if (userNameField != null) {
                    accountName = userNameField.Value;
                }
            }
            if (Provider.HasEmail) {
                var emailField = GetDefaultField(FieldTypeName.Email);
                if (emailField != null) {
                    if (!string.IsNullOrEmpty(accountName)) {
                        accountName += " - ";
                    }
                    accountName += emailField.Value;
                }
            }
            if (string.IsNullOrEmpty(accountName)) {
                accountName = "< not set >";
            }
            return accountName;
        }

        public string Notes { get; set; }

        public override string ToString() {
            return GetAccountName() + " | " + Provider.Name;
        }

        public IProvider Provider { get; set; }

        public string GetPassword() {
            return _repository.GetPassword(Id);
        }

        public void SetPassword(string password) {
            _repository.SetPassword(Id, password);
        }

        public IList<ITag> Tags { get; private set; }

        public IList<IField> Fields { get; private set; }

        public IEnumerable<IField<string>> GetFieldsByType(Contracts.FieldTypeName fieldTypeName) {
            return Fields.Select(field => field as IField<string>)
                         .Where(field => field != null)
                         .Where(field => field.FieldTypeName == fieldTypeName);
        }

        public IField<string> GetDefaultField(Contracts.FieldTypeName fieldTypeName) {
            return GetFieldsByType(fieldTypeName).FirstOrDefault(field => field.Template.IsDefault);
        }

        public virtual void Save() {
            if (IsDirty) {
                _repository.Save(this);
            }
        }

        public bool IsDirty {
            get { return (_orignalHash != GetHashCode()); }
        }

        public void SetClean() {
            _orignalHash = GetHashCode();
        }

        public override int GetHashCode() {
            unchecked {
                var result = Id.GetHashCode();
                result = (result * 397) ^ (Notes != null ? Notes.GetHashCode() : 0);
                foreach (var accountField in Fields) {
                    result = (result * 397) ^ (accountField != null ? accountField.GetHashCode() : 0);
                }
                foreach (var accountTag in Tags) {
                    result = (result * 397) ^ (accountTag != null ? accountTag.GetHashCode() : 0);
                }
                return result;
            }
        }

        public bool Equals(IAccount other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id) && Equals(other.Notes, Notes) && Equals(other.Tags, Tags);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Account)) return false;
            return Equals((Account)obj);
        }

    }

}


