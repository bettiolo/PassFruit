using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassFruit.Security.Cryptography.Net45
{
    public class Net45KeyGenerator : KeyGeneratorBase
    {
        public override Key Generate(byte[] password, Salt salt, int iterations)
        {
            using (var keyGenerator = new Rfc2898DeriveBytes(password, salt.Value, iterations))
            {
                var keyBytes = keyGenerator.GetBytes(AesBase.KeySizeInBits / 8);
                return new Key(keyBytes);
            }
        }
    }
}
