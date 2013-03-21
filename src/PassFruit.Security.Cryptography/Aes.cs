using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public abstract class Aes
    {

        public const int BlockSizeInBits = 128;
        public const int SaltSizeInBits = 192;
        public const int KeySizeInBits = 256;

        public Key DeriveMasterKey(string password, string masterSaltBase64, int masterKeyIterations)
        {
            var masterSalt = new Salt(Convert.FromBase64String(masterSaltBase64));
            return DeriveMasterKey(password, masterSalt, masterKeyIterations);
        }

        public Key DeriveNewMasterKey(string password, int masterKeyIterations)
        {
            return DeriveMasterKey(password, GenerateSalt(), masterKeyIterations);
        }

        // We need to generate a new Salt and InitializationVector for every encryption
        public Ciphertext EncryptMaster(string secretMessage, Key masterKey)
        {
            var initializationVector = GenerateInitializationVector();
            var cipherTextBytes = Encrypt(secretMessage, masterKey, initializationVector);
            return new Ciphertext(cipherTextBytes, masterKey.Salt, initializationVector);
        }

        // We need to generate a new Salt and InitializationVector for every encryption
        public Ciphertext EncryptData(string secretMessage, Key masterKey)
        {
            var initializationVector = GenerateInitializationVector();
            var dataKey = DeriveNewDataKey(masterKey);
            var cipherTextBytes = Encrypt(secretMessage, dataKey, initializationVector);
            return new Ciphertext(cipherTextBytes, dataKey.Salt, initializationVector);
        }

        public string DecryptMaster(string ciphertextBase64, Key masterKey, string masterInitializationVectorBase64)
        {
            var masterIv = new InitializationVector(Convert.FromBase64String(masterInitializationVectorBase64));
            return Decrypt(Convert.FromBase64String(ciphertextBase64), masterKey, masterIv);
        }

        public string DecryptMaster(Ciphertext ciphertext, Key masterKey)
        {
            return Decrypt(ciphertext.Value, masterKey, ciphertext.InitializationVector);
        }

        public string DecryptData(string ciphertextBase64, Key masterKey, string dataSaltBase64,
                                  string dataInitializationVectorBase64)
        {
            var dataSalt = new Salt(Convert.FromBase64String(dataSaltBase64));
            var dataKey = DeriveDataKey(masterKey, dataSalt);
            var dataIv = new InitializationVector(Convert.FromBase64String(dataInitializationVectorBase64));
            return Decrypt(Convert.FromBase64String(ciphertextBase64), dataKey, dataIv);
        }

        public string DecryptData(Ciphertext ciphertext, Key masterKey)
        {
            var dataKey = DeriveDataKey(masterKey, ciphertext.Salt);
            return Decrypt(ciphertext.Value, dataKey, ciphertext.InitializationVector);
        }

        protected Aes(RandomNumberGenerator randomNumberGenerator, Pbkdf2 pbkdf2)
        {
            RandomNumberGenerator = randomNumberGenerator;
            Pbkdf2 = pbkdf2;
        }

        protected abstract byte[] Encrypt(string secretMessage, Key secretKey, InitializationVector initializationVector);

        protected abstract string Decrypt(byte[] ciphertext, Key secretKey, InitializationVector initializationVector);

        private const int DataKeyIterations = 10;

        private RandomNumberGenerator RandomNumberGenerator { get; set; }

        private Pbkdf2 Pbkdf2 { get; set; }

        private InitializationVector GenerateInitializationVector()
        {
            return new InitializationVector(RandomNumberGenerator.Generate(BlockSizeInBits));             
        }

        private Salt GenerateSalt()
        {
            var saltBytes = RandomNumberGenerator.Generate(SaltSizeInBits);
            return new Salt(saltBytes);
        }

        public Key DeriveMasterKey(string password, Salt masterSalt, int masterKeyIterations)
        {
            var masterKey = Pbkdf2.Derive(password, masterSalt, masterKeyIterations);
            return masterKey;
        }

        private Key DeriveDataKey(Key masterKey, Salt dataSalt)
        {
            var dataKey = Pbkdf2.Derive(masterKey, dataSalt, DataKeyIterations);
            return dataKey;
        }

        private Key DeriveNewDataKey(Key masterKey)
        {
            return DeriveDataKey(masterKey, GenerateSalt());
        }

    }
}
