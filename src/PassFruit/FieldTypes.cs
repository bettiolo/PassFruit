using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PassFruit.Contracts;

namespace PassFruit {

    public class FieldTypes : IFieldTypes {

        private readonly List<IFieldType> _fieldTypes = new List<IFieldType>();

        public FieldTypes() {

        }

        public void Add(IFieldType fieldType) {
            _fieldTypes.Add(fieldType);
        }

        public IField CreateField(FieldTypeKey key, object value, Guid? fieldId = null) {
            var fieldType = _fieldTypes.SingleOrDefault(ft => ft.Key == key);
            if (fieldType == null) {
                fieldType = new FieldType(key);
            }
            var field = new Field(fieldType, fieldId);
            field.Value = value;
            return field;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<IFieldType> GetEnumerator() {
            return _fieldTypes.GetEnumerator();
        }

    }

}