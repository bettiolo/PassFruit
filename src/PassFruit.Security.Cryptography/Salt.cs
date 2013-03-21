using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class Salt
    {

        internal Salt(byte[] value)
        {
            if (value.Length != Aes.SaltSizeInBits / 8)
            {
                throw new ArgumentOutOfRangeException("value", "The size of the salt be: " + Aes.SaltSizeInBits + " bits");
            }
            Value = value;
        }

        public byte[] Value { get; private set; }

    }
}
