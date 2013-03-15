using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class Key
    {

        public Key(byte[] value)
        {
            Value = value;
        }

        public Key(string base64Value)
            : this(Convert.FromBase64String(base64Value))
        {
            
        }

        public byte[] Value { get; private set; }

    }
}
