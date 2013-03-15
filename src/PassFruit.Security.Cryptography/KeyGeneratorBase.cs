using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class KeyGeneratorBase
    {

        public Key Generate(string password, Salt salt, int iterations)
        {
            return Generate(Encoding.UTF8.GetBytes(password), salt, iterations);
        }

        public Key Generate(Key key, Salt salt, int iterations)
        {
            return Generate(key.Value, salt, iterations);
        }

        public abstract Key Generate(byte[] password, Salt salt, int iterations);

    }
}
