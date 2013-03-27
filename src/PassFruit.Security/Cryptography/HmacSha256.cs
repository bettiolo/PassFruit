using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class HmacSha256
    {

        public const int HmacSizeInBits = 256;

        public byte[] Compute(byte[] message, byte[] key)
        {
            return PlatformSpecificCompute(message, key);
        }

        protected abstract byte[] PlatformSpecificCompute(byte[] message, byte[] key);

    }
}
