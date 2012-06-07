using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IFieldTypes : IEnumerable<IFieldType> {

        IField CreateField(FieldTypeKey key, object value);

    }

}
