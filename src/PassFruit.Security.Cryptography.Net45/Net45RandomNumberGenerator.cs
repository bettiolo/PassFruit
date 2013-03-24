using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PassFruit.Security.Cryptography.Net45
{
    public class Net45RandomNumberGenerator : RandomNumberGenerator
    {

        public override byte[] Generate(int sizeInBits)
        {

            using (var randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[sizeInBits / 8];
                randomNumberGenerator.GetBytes(randomBytes);
                return randomBytes;
            }

        }

    }
}
