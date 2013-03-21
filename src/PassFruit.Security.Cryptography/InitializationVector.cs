using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class InitializationVector
    {

        internal InitializationVector(byte[] value)
        {
            if (value.Length != Aes.BlockSizeInBits/8)
            {
                throw new ArgumentOutOfRangeException("value", "The size of the initialization vector must be: " + Aes.BlockSizeInBits + " bits");
            }
            Value = value;
        }

        public byte[] Value { get; private set; }

    }
}
