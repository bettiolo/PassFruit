using System;
using System.Collections.Generic;
using System.Linq;
using PassFruit.Contracts;
using PassFruit.FieldImpl;

namespace PassFruit {

    public class Account : IAccount {

        private readonly IRepository _repository;

        private int _orignalHash;

        private readonly List<ITag> _tags;

        private readonly List<IField> _fields;

        internal Account(IRepository repository, Guid? id = null) {
            _repository = repository;
            _tags = new List<ITag>();
            _fields = new List<IField>();
            Id = id.HasValue ? id.Value : Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public virtual string GetAccountName() {
            var accountName = "";
            if (Provider.HasUserName) {
                var userNameField = GetDefaultField(FieldTypeKey.UserName);
                if (userNameField != null) {
                    accountName = userNameField.Value;
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

        public IProvider Provider { get; set; }

        public string GetPassword() {
            return _repository.GetPassword(Id);
        }

        public void SetPassword(string password) {
            _repository.SetPassword(Id, password);
        }

        public IEnumerable<ITag> Tags { get { return _tags.ToArray(); } }

        public IEnumerable<IField> Fields { get { return _fields.ToArray(); } }

        public IEnumerable<IField<string>> GetFieldsByKey(FieldTypeKey fieldTypeKey) {
            return _fields.Select(field => field as IField<string>)
                          .Where(field => field != null)
                          .Where(field => field.FieldType.Key == fieldTypeKey);
        }

        public IField<string> GetDefaultField(FieldTypeKey fieldTypeKey) {
            return GetFieldsByKey(fieldTypeKey).FirstOrDefault(field => field.FieldType.IsDefault);
        }

        public void SetField<TValue>(FieldTypeKey fieldTypeKey, TValue value) {
            var field = (IField<TValue>)_fields.SingleOrDefault(f => f.FieldType.Key == fieldTypeKey);
            if (field == null) {
                var newField = _repository.FieldTypes.CreateField(fieldTypeKey, value);
                _fields.Add(newField);
            } else {
                field.Value = value;
            }
        }

        public void AddTag(string tagName) {
            _tags.Add(_repository.Tags.Create(tagName));
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
                foreach (var field in Fields) {
                    result = (result * 397) ^ (field != null ? field.GetHashCode() : 0);
                }
                foreach (var tag in Tags) {
                    result = (result * 397) ^ (tag != null ? tag.GetHashCode() : 0);
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


