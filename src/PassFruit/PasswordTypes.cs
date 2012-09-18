using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PassFruit.Contracts;

namespace PassFruit {

    public class PasswordTypes : IPasswordTypes {

        private readonly List<IPasswordType> _passwordTypes = new List<IPasswordType>();

        //public IField CreateField(PasswordTypeKey key, object value) {
        //    var fieldType = _passwordTypes.SingleOrDefault(ft => ft.Key == key);
        //    if (fieldType == null) {
        //        fieldType = new FieldType(key);
        //    }
        //    var field = new Password(fieldType);
        //    field.Value = value;
        //    return field;
        //}

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<IPasswordType> GetEnumerator() {
            return _passwordTypes.GetEnumerator();
        }

    }

}