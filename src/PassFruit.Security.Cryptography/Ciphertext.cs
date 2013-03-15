using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class Ciphertext
    {

        public Ciphertext(byte[] value)
        {
            Value = value;
        }

        public Ciphertext(string base64value)
            : this(Convert.FromBase64String(base64value))
        {
            
        }

        public byte[] Value { get; private set; }

        public string ToBase64()
        {
            return Convert.ToBase64String(Value);
        }
    
    }
}
