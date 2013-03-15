using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class InitializationVector
    {

        public InitializationVector(byte[] value)
        {
            Value = value;
        }

        public InitializationVector(string base64Value)
            : this(Convert.FromBase64String(base64Value))
        {
            
        }

        public InitializationVector(RandomNumberGeneratorBase randomNumberGenerator)
            : this(randomNumberGenerator.Get(AesBase.InitializationVectorSizeInBits))
        {

        }

        public byte[] Value { get; private set; }

    }
}
