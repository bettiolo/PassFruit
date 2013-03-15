using System;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PassFruit.Security.Cryptography.Net45
{
    public class Net45Aes : AesBase
    {

        public Net45Aes() : base(new Net45RandomNumberGenerator()) { }

        public InitializationVector GenerateRandomInitializationVector()
        {
            return new InitializationVector(RandomNumberGenerator.Get(BlockSizeInBits));
        }

        public Salt GenerateRandomSalt()
        {
            return new Salt(RandomNumberGenerator);
        }

        private AesCryptoServiceProvider CreateAes(Key secretKey, InitializationVector initializationVector)
        {
            return new AesCryptoServiceProvider
            {
                KeySize = KeySizeInBits,
                BlockSize = BlockSizeInBits,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = secretKey.Value,
                IV = initializationVector.Value
            };
        }

        public override Ciphertext Encrypt(string secretMessage, Key secretKey, InitializationVector initializationVector)
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
                return new Ciphertext(memoryStream.ToArray());
            }
        }

        public override string Decrypt(Ciphertext ciphertext, Key secretKey, InitializationVector initializationVector)
        {
            using (var aes = CreateAes(secretKey, initializationVector))
            using (var decryptor = aes.CreateDecryptor())
            using (var memoryStream = new MemoryStream(ciphertext.Value))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }

    }

}
