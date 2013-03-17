using System;
using System.Collections.Generic;
using PassFruit.Contracts;

namespace PassFruit {

    public class Field : IField {

        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public Field(IFieldType fieldType, Guid? id = null, string name = "") {
            FieldType = fieldType;
            Id = id ?? Guid.NewGuid();
            Name = name;
            if (string.IsNullOrWhiteSpace(Name)) {
                Name = FieldType.Key.ToString();
            }
        }

        public IFieldType FieldType { get; private set; }

        public Guid Id { get; private set; }

        public string Name { get; set; }
        
        public object Value { get; set; }

        public IDictionary<string, object> GetProperties()
        {
            return _properties;
        }

        public void SetProperty(string name, object value)
        {
            _properties[name] = value;
        }

        public override string ToString() {
            return Name;
        }

        public override int GetHashCode() {
            unchecked {
                int result = (FieldType != null ? FieldType.GetHashCode() : 0);
                result = (result * 397) ^ Id.GetHashCode();
                result = (result * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                result = (result * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return result;
            }
        }

        public bool Equals(Field other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.FieldType, FieldType) 
                && other.Id.Equals(Id) 
                && Equals(other.Value, Value)
                && Equals(other.Name, Name);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Field)) return false;
            return Equals((Field) obj);
        }

    }

}
