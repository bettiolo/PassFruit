using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.FieldImpl {

    public class Field<TValue> : IField<TValue> {

        private readonly Type _valueType;

        public Field(IFieldType fieldType, Type valueType) {
            _valueType = valueType;
            FieldType = fieldType;
        }

        public Type ValueType {
            get { return _valueType; }
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
        
        public TValue Value { get; set; }

        public override string ToString() {
            return Name;
        }

    }

}
