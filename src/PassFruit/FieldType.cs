using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class FieldType : IFieldType {

        internal FieldType(FieldTypeKey key) {
            Key = key;
        }

        public FieldTypeKey Key { get; private set; }

        public bool IsDefault { get; set; }

        public bool IsPassword { get; set; }

    }

}
