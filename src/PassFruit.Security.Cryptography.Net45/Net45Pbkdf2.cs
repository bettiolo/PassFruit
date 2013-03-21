using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PassFruit.Security.Cryptography.Net45
{
    public class Net45Pbkdf2 : Pbkdf2
    {

        protected override byte[] DeriveBytes(byte[] password, Salt salt, int iterations)
        {
            using (var keyGenerator = new Rfc2898DeriveBytes(password, salt.Value, iterations))
            {
                return keyGenerator.GetBytes(Aes.KeySizeInBits / 8);
            }
        }

    }
}
