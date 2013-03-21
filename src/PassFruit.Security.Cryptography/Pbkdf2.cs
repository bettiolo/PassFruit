using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{

    /// <summary>
    /// RFC2898 
    /// Implements password-based key derivation functionality, PBKDF2, by using pseudo-random
    /// number generator based on HMACSHA1, hash-based (SHA-1) message authentication code
    /// </summary>
    public abstract class Pbkdf2
    {

        internal Key Derive(string password, Salt salt, int iterations)
        {
            // return Derive(password.GetBytes(), salt, iterations);
            return Derive(Encoding.UTF8.GetBytes(password), salt, iterations);
        }

        internal Key Derive(Key key, Salt salt, int iterations)
        {
            return Derive(key.Value, salt, iterations);
        }

        internal Key Derive(byte[] bytes, Salt salt, int iterations)
        {
            var keyBytes = DeriveBytes(bytes, salt, iterations);
            return new Key(keyBytes, salt, iterations);
        }

        protected abstract byte[] DeriveBytes(byte[] password, Salt salt, int iterations);

    }
}
