using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class AesBase
    {

        public const int InitializationVectorSizeInBits = BlockSizeInBits;
        public const int BlockSizeInBits = 128;
        public const int SaltSizeInBits = 192;
        public const int KeySizeInBits = 256;

        public const int MasterKeyIterations = 1337;

        public AesBase(RandomNumberGeneratorBase randomNumberGenerator)
        {
            RandomNumberGenerator = randomNumberGenerator;
        }

        protected RandomNumberGeneratorBase RandomNumberGenerator { get; private set; }

        public abstract Ciphertext Encrypt(string secretMessage, Key secretKey, InitializationVector initializationVector);

        public abstract string Decrypt(Ciphertext ciphertext, Key secretKey, InitializationVector initializationVector);

    }
}
