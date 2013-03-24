using System;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PassFruit.Security.Cryptography.Net45
{
    public class Net45Aes : Aes
    {

        public Net45Aes() : base(new Net45RandomNumberGenerator()) { }

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

        public override byte[] Encrypt(string message, byte[] key, byte[] initializationVector)
        {
            // ToDo: Check input
            using (var aes = CreateAes(key, initializationVector))
            using (var encryptor = aes.CreateEncryptor())
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(message);
                }
                return memoryStream.ToArray();
            }
        }

        public override string Decrypt(byte[] ciphertext, byte[] secretKey, byte[] initializationVector)
        {
            // ToDo: Check input
            //if (!CheckInput(ciphertext, secretKey.Value, initializationVector.Value))
            //{
            //    return null;
            //}
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
