using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PassFruit.Security.Cryptography.Net45
{
    public class Net45HmacSha256 : HmacSha256
    {

        protected override byte[] PlatformSpecificCompute(byte[] message, byte[] key)
        {
            using (var hmacsha256 = new HMACSHA256(key))
            {
                return hmacsha256.ComputeHash(message);
            }
        }

    }
}
