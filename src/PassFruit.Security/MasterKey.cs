using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Security.Cryptography;

namespace PassFruit.Security
{
    public class MasterKey
    {

        public MasterKey(string secretPassword, int iterations, Pbkdf2 pbkdf2)
            : this(secretPassword, pbkdf2.GenerateSalt(), iterations, pbkdf2)
        {

        }

        public MasterKey(string secretPassword, byte[] salt, int iterations, Pbkdf2 pbkdf2)
        {
            Salt = salt;
            SecretKey = pbkdf2.Compute(secretPassword, Salt, iterations);
            Iterations = iterations;
        }

        /// <summary>
        /// The value of the SecretKey should never be stored locally or transmitted on the wire
        /// </summary>
        public byte[] SecretKey { get; private set; }

        public byte[] Salt { get; private set; }

        public int Iterations { get; private set; }

    }
}
