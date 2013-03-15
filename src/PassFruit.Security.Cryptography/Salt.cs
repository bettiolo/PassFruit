using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class Salt
    {

        public Salt(byte[] value)
        {
            Value = value;
        }

        public Salt(string base64Value)
            : this(Convert.FromBase64String(base64Value))
        {
            
        }

        public Salt(RandomNumberGeneratorBase randomNumberGenerator)
            : this(randomNumberGenerator.Get(AesBase.SaltSizeInBits))
        {
            
        }

        public byte[] Value { get; private set; }

    }
}
