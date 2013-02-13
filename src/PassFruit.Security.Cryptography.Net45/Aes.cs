using System;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PassFruit.Security.Cryptography.Net45
{
    public class Aes
    {

        public const int KeySizeInBits = 256;
        public const int BlockSizeInBits = 128;
        public const int SaltSizeInBits = 192;
        public const int Iterations = 1337;

        // aka PBKDF2
        public byte[] GenerateKey(string secretPassword, byte[] salt)
        {
            using (var keyGenerator = new Rfc2898DeriveBytes(secretPassword, salt, Iterations))
            {
                var keyBytes = keyGenerator.GetBytes(KeySizeInBits / 8);
                return keyBytes;
            }
        }

        //public byte[] GenerateRandomInitializationVector()
        //{
        //    var initializationVector = new byte[BlockSize / 8];
        //    var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        //    rngCryptoServiceProvider.GetBytes(initializationVector);
        //    return initializationVector;
        //}
        
        public byte[] GenerateRandomInitializationVector()
        {
            return GenerateRandomBytes(BlockSizeInBits);
        }

        public byte[] GenerateRandomSalt()
        {
            return GenerateRandomBytes(SaltSizeInBits);
        }

        private byte[] GenerateRandomBytes(int sizeInBits)
        {
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var initializationVector = new byte[sizeInBits / 8];
                randomNumberGenerator.GetBytes(initializationVector);
                return initializationVector;
            }
        }


        private AesCryptoServiceProvider CreateAes(byte[] secretKey, byte[] initializationVector)
        {
            return new AesCryptoServiceProvider
            {
                KeySize = KeySizeInBits,
                BlockSize = BlockSizeInBits,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = secretKey,
                IV = initializationVector
            };
        }

        public string EncryptToBase64(string secretMessage, byte[] secretKey, byte[] initializationVector)
        {
            var ciphertext = Encrypt(secretMessage, secretKey, initializationVector);
            return Convert.ToBase64String(ciphertext);
        }

        public byte[] Encrypt(string secretMessage, byte[] secretKey, byte[] initializationVector)
        {
            using (var aes = CreateAes(secretKey, initializationVector))
            using (var encryptor = aes.CreateEncryptor())
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(secretMessage);
                }
                return memoryStream.ToArray();
            }
        }

        public string DecryptFromBase64(string ciphertext, byte[] secretKey, byte[] initializationVector)
        {
            return Decrypt(Convert.FromBase64String(ciphertext), secretKey, initializationVector);
        }

        public string Decrypt(byte[] ciphertext, byte[] secretKey, byte[] initializationVector)
        {
            using (var aes = CreateAes(secretKey, initializationVector))
            using (var decryptor = aes.CreateDecryptor())
            using (var memoryStream = new MemoryStream(ciphertext))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }

    }

}
