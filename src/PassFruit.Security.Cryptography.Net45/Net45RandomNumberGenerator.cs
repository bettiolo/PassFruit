using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassFruit.Security.Cryptography.Net45
{
    public class Net45RandomNumberGenerator : RandomNumberGeneratorBase
    {
        public override byte[] Get(int sizeInBits)
        {
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[sizeInBits / 8];
                randomNumberGenerator.GetBytes(randomBytes);
                return randomBytes;
            }
        }
    }
}
