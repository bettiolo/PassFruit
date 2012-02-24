using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.FieldImpl {

    public class Field<TValue> : IField<TValue> {

        public Field(IFieldType fieldType) {
            FieldType = fieldType;
        }

        public IFieldType FieldType { get; private set; }

        public string Name { get; set; }
        
        public TValue Value { get; set; }

    }

}
