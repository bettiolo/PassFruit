using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class RandomNumberGeneratorBase
    {

        public abstract byte[] Get(int sizeInBits);

    }
}
