using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    /// <summary>
    /// RFC2898 
    /// Implements password-based key derivation function 2, PBKDF2, by using pseudo-random
    /// number generator based on HMACSHA1, hash-based (SHA-1) message authentication code
    /// </summary>
    public abstract class Pbkdf2
    {

        public const int SaltSizeInBits = 256;
        public const int MinumumIterations = 10;

        private readonly RandomNumberGenerator _randomNumberGenerator;

        public Pbkdf2(RandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public byte[] Compute(string password, byte[] salt, int iterations)
        {
            return Compute(Encoding.UTF8.GetBytes(password), salt, iterations);
        }

        public byte[] Compute(byte[] password, byte[] salt, int iterations)
        {
            if (salt == null || salt.Length != SaltSizeInBits / 8)
            {
                throw new ArgumentException("The salt size must be " + SaltSizeInBits + " bits", "salt");
            }
            if (iterations < MinumumIterations)
            {
                throw new ArgumentException("The minimum number of iteration is " + MinumumIterations, "iterations");
            }
            return PlatformSpecificCompute(password, salt, iterations);
        }

        protected abstract byte[] PlatformSpecificCompute(byte[] password, byte[] salt, int iterations);

        public byte[] GenerateSalt()
        {
            return _randomNumberGenerator.Generate(SaltSizeInBits);
        }

    }
}
