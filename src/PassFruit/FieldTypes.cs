using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PassFruit.Contracts;
using PassFruit.FieldImpl;

namespace PassFruit {

    public class FieldTypes : IFieldTypes {

        private readonly IRepository _repository;

        private readonly List<IFieldType> _fieldTypes = new List<IFieldType>();

        public FieldTypes(IRepository repository) {
            _repository = repository;
        }

        public IEnumerator<IFieldType> GetEnumerator() {
            return _fieldTypes.GetEnumerator();
        }

        public IField<TValue> CreateField<TValue>(FieldTypeKey key, TValue value) {
            var fieldType = _repository.FieldTypes.SingleOrDefault(ft => ft.Key == key);
            if (fieldType == null) {
                fieldType = new FieldType(key);
            }
            var field = new Field<TValue>(fieldType, typeof(TValue));
            field.Value = value;
            return field;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

    }

}