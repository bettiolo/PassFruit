using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Security.Cryptography;

namespace PassFruit.Security
{
    public class Encryptor
    {

        private readonly Pbkdf2 _pbkdf2;

        private readonly Aes _aes;

        public Encryptor(Pbkdf2 pbkdf2, Aes aes)
        {
            _pbkdf2 = pbkdf2;
            _aes = aes;
        }

        public EncryptedDataDto EncryptData(string message, byte[] masterKey, int iterations)
        {
            var salt = _pbkdf2.GenerateSalt();
            var dataKey = _pbkdf2.Compute(masterKey, salt, iterations);
            var initializationVector = _aes.GenerateInitializationVector();
            var ciphertext = _aes.Encrypt(message, dataKey, initializationVector); ;
            return new EncryptedDataDto
            {
                Iterations = iterations,
                Salt = salt,
                InitializationVector = initializationVector,
                Ciphertext = ciphertext
            };
        }

    }
}
