using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IField {

        string Name { get; set; }

        IFieldType FieldType { get; }

        object Value { get; set; }

    }

}
