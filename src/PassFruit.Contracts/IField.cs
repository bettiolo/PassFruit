using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IField {

        Guid Id { get; }

        string Name { get; set; }

        IFieldType FieldType { get; }

        object Value { get; set; }

        IDictionary<string, object> GetProperties();

        void SetProperty(string name, object value);

    }

}
