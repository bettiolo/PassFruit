using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PassFruit.Contracts;
using PassFruit.DataStore;

namespace PassFruit {

    public class Account : IAccount {

        private int _orignalHash;

        private IProvider _provider; 

        private readonly IFieldTypes _fieldTypes;

        private readonly IList<IField> _fields;

        internal Account(IFieldTypes fieldTypes) {
            Tags = new Tags(Id);
            _fields = new List<IField>();
            _fieldTypes = fieldTypes;
        }

        internal void Load(AccountDto accountDto) {
            Id = accountDto.Id;
            _provider = new Provider(accountDto.ProviderKey);
            foreach (var fieldDto in accountDto.Fields) {
                var field = new Field(
                        new FieldType(fieldDto.FieldTypeKey.ToEnum<FieldTypeKey>()),
                        fieldDto.Id,
                        fieldDto.Name) {
                    Value = fieldDto.Value
                };
                _fields.Add(field);
            }
            foreach (var tagDto in accountDto.Tags) {
                Tags.Add(tagDto.Key);
            }
            Notes = accountDto.Notes;
            LastChangedUtc = accountDto.LastChangedUtc;

            SetClean();
        }

        internal void Update(AccountDto accountDto) {
            if (accountDto.Id != Id) {
                throw new Exception("Cannot update a different DTO, ID don't match");
            };
            accountDto.Fields.Clear();
            foreach (var field in Fields) {
                var fieldDto = new FieldDto {
                    Id = field.Id,
                    FieldTypeKey = field.FieldType.Key.ToString(), 
                    Name = field.Name, 
                    Value = field.Value
                };
                accountDto.Fields.Add(fieldDto);
            }

            accountDto.Tags.Clear();
            foreach (var tag in Tags) {
                accountDto.Tags.Add(new TagDto {
                    Key = tag.Key
                });
            }
            accountDto.Notes = Notes;

        }

        public Guid Id { get; private set; }

        public string EncryptionSalt {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string GetAccountName() {
            var accountName = "";
            if (Provider.HasUserName) {
                var userNameField = GetFieldsByKey(FieldTypeKey.UserName).FirstOrDefault();
                if (userNameField != null) {
                    accountName = userNameField.Value.ToString();
                }
            }
            if (Provider.HasEmail) {
                var emailField = GetFieldsByKey(FieldTypeKey.Email).FirstOrDefault();
                if (emailField != null) {
                    if (!string.IsNullOrEmpty(accountName)) {
                        accountName += " - ";
                    }
                    accountName += emailField.Value;
                }
            }
            if (string.IsNullOrEmpty(accountName)) {
                if (Fields.Any()) {
                    accountName = Fields.First().Value.ToString();
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

        public ITags Tags { get; private set; }

        public IEnumerable<IField> Fields { get { return _fields.ToArray(); } }

        public IEnumerable<IField> GetFieldsByKey(FieldTypeKey fieldTypeKey) {
            return _fields.Where(field => field.FieldType.Key == fieldTypeKey);
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

        public bool IsDirty {
            get { return (_orignalHash != GetHashCode()); }
        }

        private void SetClean() {
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


