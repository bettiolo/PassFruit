using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class Key
    {

        internal Key(byte[] value, Salt salt, int iterations)
        {
            if (value.Length != Aes.KeySizeInBits / 8)
            {
                throw new ArgumentOutOfRangeException("value", "The size of the key must be: " + Aes.KeySizeInBits + " bits");
            }
            Value = value;
            Salt = salt;
            Iterations = iterations;
        }

        public byte[] Value { get; private set; }

        public Salt Salt { get; private set; }

        public int Iterations { get; private set; }

    }
}
