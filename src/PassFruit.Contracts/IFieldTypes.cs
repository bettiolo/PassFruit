using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IFieldTypes : IEnumerable<IFieldType> {

        void Add(IFieldType fieldType);

        IField CreateField(FieldTypeKey key, object value, Guid? fieldId = null);

    }

}
