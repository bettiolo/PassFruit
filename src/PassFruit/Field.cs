using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.FieldImpl {

    public class Field : IField {

        public Field(IFieldType fieldType) {
            FieldType = fieldType;
        }

        public IFieldType FieldType { get; private set; }

        private string _name;
        public string Name { 
            get {
                if (string.IsNullOrEmpty(_name)) {
                    return FieldType.Key.ToString();
                }
                return _name;
            }
            set { _name = value; } 
        }
        
        public object Value { get; set; }

        public override string ToString() {
            return Name;
        }

        public override int GetHashCode() {
            unchecked {
                int result = (FieldType != null ? FieldType.GetHashCode() : 0);
                result = (result * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                result = (result * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return result;
            }
        }

    }

}
