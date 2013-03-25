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

        public byte[] Encrypt(string message, byte[] key, byte[] initializationVector)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("An empty message should not be encrypted", "message");
            }
            CheckInput(key, initializationVector);
            return PlatformSpecificEncrypt(message, key, initializationVector);
        }

        protected abstract byte[] PlatformSpecificEncrypt(string message, byte[] key, byte[] initializationVector);

        public string Decrypt(byte[] ciphertext, byte[] key, byte[] initializationVector)
        {
            if (ciphertext == null || ciphertext.Length == 0)
            {
                throw new ArgumentException("Cannot decrypt an empty ciphertext", "ciphertext");
            }
            CheckInput(key, initializationVector);
            return PlatformSpecificDecrypt(ciphertext, key, initializationVector);
        }

        protected abstract string PlatformSpecificDecrypt(byte[] ciphertext, byte[] key, byte[] initializationVector);

        public byte[] GenerateInitializationVector()
        {
            return _randomNumberGenerator.Generate(BlockSizeInBits);
        }

        private void CheckInput(byte[] key, byte[] initializationVector)
        {
            if (key == null || key.Length != KeySizeInBits / 8 )
            {
                throw new ArgumentException("Cannot use an empty or invalid key", "key");
            }
            if (initializationVector == null || initializationVector.Length != BlockSizeInBits / 8)
            {
                throw new ArgumentException("Cannot use an empty or invalid initialization vectory", "initializationVector");
            }
        }

    }
}
