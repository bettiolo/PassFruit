using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class Aes
    {

        private readonly RandomNumberGenerator _randomNumberGenerator;

        public const int BlockSizeInBits = 128;
        public const int KeySizeInBits = 256;

        protected Aes(RandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public abstract byte[] Encrypt(string message, byte[] key, byte[] initializationVector);

        public abstract string Decrypt(byte[] ciphertext, byte[] secretKey, byte[] initializationVector);

        public byte[] GenerateInitializationVector()
        {
            return _randomNumberGenerator.Generate(BlockSizeInBits);
        }

        protected bool CheckInput(byte[] ciphertext, byte[] key, byte[] initializationVector)
        {

            if (ciphertext == null || key == null || initializationVector == null)
            {
                return false;
            }

            if (key.Length != KeySizeInBits / 8)
            {
                return false;
            }

            if (initializationVector.Length != BlockSizeInBits / 8)
            {
                return false;
            }

            return true;
        }

    }
}
