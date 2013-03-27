using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class RandomNumberGenerator
    {
        private const int MinimumSizeInBits = 128;

        public byte[] Generate(int sizeInBits)
        {
            if (sizeInBits < MinimumSizeInBits)
            {
                throw new ArgumentException("The minimum size that can be generated is " + MinimumSizeInBits + " bits", "sizeInBits");
            }
            return  PlatformSpecificGenerate(sizeInBits);
        }

        protected abstract byte[] PlatformSpecificGenerate(int sizeInBits);

    }
}
