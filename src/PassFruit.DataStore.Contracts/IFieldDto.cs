using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.DataStore.Contracts {

    public interface IFieldDto {

        Guid Id { get; }

        string Name { get; set; }

        string FieldTypeKey { get; }

        object Value { get; set; }

        bool Equals(IFieldDto otherFieldDto);

    }

}
