using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PassFruit.Contracts;
using PassFruit.DataStore;
using PassFruit.DataStore.Contracts;

namespace PassFruit {

    public class Account : IAccount {

        private int _orignalHash;

        private IProvider _provider;

        private readonly IList<IPassword> _passwords; 

        private readonly IFieldTypes _fieldTypes;

        private readonly IPasswordTypes _passwordTypes;

        private readonly IList<IField> _fields;

        internal Account(IFieldTypes fieldTypes, IPasswordTypes passwordTypes) {
            Tags = new Tags(Id);
            _fields = new List<IField>();
            _passwords = new List<IPassword>();
            _fieldTypes = fieldTypes;
            _passwordTypes = passwordTypes;
            // _provider = new Provider();
        }

        internal void Load(IAccountDto accountDto, IEnumerable<IPasswordDto> passwordDtos) {
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

            foreach (var passwordDto in passwordDtos) {
                _passwords.Add(new Password(passwordDto));
            }

            SetClean();
        }

        internal void Update(IAccountDto accountDto, IList<IPasswordDto> passwordDtos) {
            if (accountDto.Id != Id) {
                throw new Exception("Cannot update a different DTO, ID don't match");
            };
            accountDto.Fields.Clear();
            foreach (var field in Fields) {
                var fieldDto = new FieldDto(field.Id) {
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

            passwordDtos.Clear();
            foreach (var password in _passwords) {
                passwordDtos.Add(new PasswordDto(password.Id) {
                    Name = password.Name,
                    Password = password.Value
                });
            }

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

        public IEnumerable<IPassword> GetPasswords() {
            return _passwords.ToArray();
        }

        public void SetPassword(string password, string name) {
            throw new NotSupportedException();
        }

        public ITags Tags { get; private set; }

        public IEnumerable<IField> Fields { get { return _fields.ToArray(); } }

        public IEnumerable<IField> GetFieldsByKey(FieldTypeKey fieldTypeKey) {
            return _fields.Where(field => field.FieldType.Key == fieldTypeKey);
        }

        //public IField GetDefaultField(FieldTypeKey fieldTypeKey) {
        //    var fields = GetFieldsByKey(fieldTypeKey);
        //    //var fieldsCount = fields.Count();
        //    //if (fieldsCount > 0
        //    //    && (fieldsCount == 1 /*|| !fields.Any((field => field.FieldType.IsDefault))*/)) {
        //    //    return fields.First();
        //    //} 
        //    return fields.FirstOrDefault(/*field => field.FieldType.IsDefault*/);
        //}

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


