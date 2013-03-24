using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class RandomNumberGenerator
    {

        public abstract byte[] Generate(int sizeInBits);

    }
}
