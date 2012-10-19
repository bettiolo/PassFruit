using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Fields
{
    public class PasswordField : Field
    {
        public PasswordField(IFieldType fieldType, Guid? id = null, string name = "") 
            : base(fieldType, id, name)
        {
        }
    }
}
