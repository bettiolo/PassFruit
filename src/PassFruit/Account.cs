using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PassFruit.Contracts;
using PassFruit.DataStore.Contracts;

namespace PassFruit {

    public class Account : IAccount {

        private int _orignalHash;

        private readonly IProvider _provider;

        private readonly IList<IPassword> _passwords; 

        private readonly IFieldTypes _fieldTypes;

        private readonly IList<IField> _fields;

        private Account() {
            Tags = new Tags(Id);
            _fields = new List<IField>();
            _passwords = new List<IPassword>();
        }

        internal Account(IAccountDto accountDto, IEnumerable<IPasswordDto> passwordDtos, IFieldTypes fieldTypes) : this() {
            _provider = new Provider(accountDto.ProviderKey);
            _fieldTypes = fieldTypes;
            Id = accountDto.Id;
            foreach (var fieldDto in accountDto.Fields) {
                _fields.Add(new Field(
                    new FieldType((FieldTypeKey)Enum.Parse(typeof(FieldTypeKey), fieldDto.FieldTypeKey, true)),
                    fieldDto.Id,
                    fieldDto.Name
                ));
            }
            foreach (var tag in accountDto.Tags) {
                Tags.Add(tag.Name);
            }
            foreach (var passwordDto in passwordDtos) {
                _passwords.Add(new Password(passwordDto));
            }
            Notes = "";
        }

        //internal Account(IPasswords passwords, IProvider provider, IFieldTypes fieldTypes, DateTime lastChangedUtc, 
        //                 Guid? id = null) : this() {
        //    _passwords = passwords;
        //    _provider = provider;
        //    _fieldTypes = fieldTypes;
        //    Id = id.HasValue ? id.Value : Guid.NewGuid();
        //    _fields = new List<IField>();
        //    Tags = new Tags(Id);
        //    Notes = "";
        //}

        public Guid Id { get; private set; }

        public string EncryptionSalt {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string GetAccountName() {
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

        public IProvider Provider {
            get { return _provider; }
        }

        public IPassword GetPassword(Guid passwordId) {
            return _passwords.First(password => password.Id == passwordId);
        }

        public void SetPassword(string password, string name) {
            throw new NotSupportedException();
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
                var newField = _fieldTypes.CreateField(fieldTypeKey, value);
                _fields.Add(newField);
            } else {
                field.Value = value;
            }
        }

        public DateTime LastChangedUtc { get; private set; }

        public void AddTag(string tagKey) {
            Tags.Add(tagKey);
        }

        public void DeleteTag(string tagKey) {
            Tags.Remove(tagKey);
        }

        public void DeleteAllPasswords() {
            _passwords.Clear();
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
                foreach (var password in _passwords) {
                    result = (result * 397) ^ (password != null ? password.GetHashCode() : 0);
                }
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


