using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class FieldType : IFieldType {

        internal FieldType(FieldTypeKey key, string description = "") {
            Key = key;
            Description = description;
        }

        public FieldTypeKey Key { get; private set; }

        public string Description { get; private set; }

        public override string ToString() {
            return Description;
        }

    }

}
