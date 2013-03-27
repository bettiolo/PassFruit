using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PassFruit.Security.Cryptography.Net45
{

    public class Net45Pbkdf2 : Pbkdf2
    {

        public Net45Pbkdf2() 
            : base(new Net45RandomNumberGenerator())
        {

        }

        protected override byte[] PlatformSpecificCompute(byte[] password, byte[] salt, int iterations)
        {
            using (var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return keyGenerator.GetBytes(Aes.KeySizeInBits / 8);
            }
        }

    }

}
