using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class HmacSha256
    {

        private readonly RandomNumberGenerator _randomNumberGenerator;

        public const int KeySizeInBits = 512;
        public const int HmacSizeInBits = 256;

        protected HmacSha256(RandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public abstract byte[] Compute(byte[] message, byte[] key);

    }
}
