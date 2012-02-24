using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.FieldImpl {

    public class StringField : IField<string> {

        public string Name { get; set; }

        public IFieldType FieldType { get; set; }

        public FieldTypeName FieldTypeName { get; set; }

        public string Value { get; set; }

    }

}
