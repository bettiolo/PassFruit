using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PassFruit.Contracts;
using PassFruit.FieldImpl;

namespace PassFruit {

    public class Account : IAccount {

        private readonly IRepository _repository;

        private int _orignalHash;

        private readonly List<IField> _fields;

        internal Account(IRepository repository, IProvider provider, Guid? id = null) {
            _repository = repository;
            _provider = provider;
            Id = id.HasValue ? id.Value : Guid.NewGuid();
            _fields = new List<IField>();
            Tags = new Tags(_repository);
            Notes = "";
        }

        public Guid Id { get; private set; }

        public string EncryptionSalt {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string GetAccountName() {
            var accountName = "";
            if (Provider.HasUserName) {
                var userNameField = GetDefaultField(FieldTypeKey.UserName);
                if (userNameField != null) {
                    accountName = userNameField.Value.ToString();
                }
            }
            if (Provider.HasEmail) {
                var emailField = GetDefaultField(FieldTypeKey.Email);
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

        private readonly IProvider _provider;
        public IProvider Provider {
            get { return _provider; }
        }

        public string GetPassword(string passwordKey = "") {
            return _repository.GetPassword(Id, passwordKey);
        }

        public void SetPassword(string password, string passwordKey = "") {
            _repository.SetPassword(Id, password, passwordKey);
        }

        public ITags Tags { get; private set; }

        public IEnumerable<IField> Fields { get { return _fields.ToArray(); } }

        public IEnumerable<IField> GetFieldsByKey(FieldTypeKey fieldTypeKey) {
            return _fields.Where(field => field.FieldType.Key == fieldTypeKey);
        }

        public IField GetDefaultField(FieldTypeKey fieldTypeKey) {
            var fields = GetFieldsByKey(fieldTypeKey);
            var fieldsCount = fields.Count();
            if (fieldsCount > 0 
                && (fieldsCount == 1 || !fields.Any((field => field.FieldType.IsDefault)))) {
                return fields.First();
            } 
            return fields.FirstOrDefault(field => field.FieldType.IsDefault);
        }

        public void SetField(FieldTypeKey fieldTypeKey, object value) {
            var field = _fields.SingleOrDefault(f => f.FieldType.Key == fieldTypeKey);
            if (field == null) {
                var newField = _repository.FieldTypes.CreateField(fieldTypeKey, value);
                _fields.Add(newField);
            } else {
                field.Value = value;
            }
        }

        public void AddTag(string tagKey) {
            Tags.Add(tagKey);
        }

        public void DeleteTag(string tagKey) {
            Tags.Remove(tagKey);
        }

        public void DeleteAllPasswords() {
            _repository.DeletePasswords(Id);
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
                Debug.WriteLine("Account ID: " + Id);

                result = (result * 397) ^ (Notes != null ? Notes.GetHashCode() : 0);
                Debug.WriteLine(" - HashCode after notes: " + result);
                foreach (var field in Fields) {
                    result = (result * 397) ^ (field != null ? field.GetHashCode() : 0);
                }
                Debug.WriteLine(" - Fields: " + string.Join(", ", Fields.Select(field => field.Name + "<" + field.FieldType.Key + "> " + field.Value).ToArray()));
                Debug.WriteLine(" - HashCode after fields: " + result);
                foreach (var tag in Tags) {
                    result = (result * 397) ^ (tag != null ? tag.GetHashCode() : 0);
                }
                Debug.WriteLine(" - Tags: " + string.Join(", ", Tags.Select(tag => tag.Key).ToArray()));
                Debug.WriteLine(" - HashCode after tags: " + result);
                return result;
            }
        }

        public bool Equals(IAccount other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.GetHashCode() == GetHashCode();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Account)) return false;
            return Equals((Account)obj);
        }

    }

}


