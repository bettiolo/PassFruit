using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    internal class FieldType : IFieldType {

        public string Type { get; set; }

        public bool IsDefault { get; set; }

        public bool IsPassword { get; set; }

    }

}
