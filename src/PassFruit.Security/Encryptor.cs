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

        public EncryptedData EncryptData(string message, byte[] masterKey, int dataKeyIterations)
        {
            var salt = _pbkdf2.GenerateSalt();
            var dataKey = _pbkdf2.Compute(masterKey, salt, dataKeyIterations);
            var initializationVector = _aes.GenerateInitializationVector();
            var ciphertext = _aes.Encrypt(message, dataKey, initializationVector);
            return new EncryptedData(salt, initializationVector, dataKeyIterations, ciphertext);
        }

        public string DecryptData(EncryptedData encryptedData, byte[] masterKey)
        {
            var dataKey = _pbkdf2.Compute(masterKey, encryptedData.Salt, encryptedData.Iterations);
            var message = _aes.Decrypt(encryptedData.Ciphertext, dataKey, encryptedData.InitializationVector);
            return message;
        }

    }
}
