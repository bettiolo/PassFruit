using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace PassFruit.Security.Cryptography.Net45.Tests
{
    [TestFixture]
    public abstract class AesTestsBase
    {

        private const string KnownSecret = "Test of a secret message";
        private const string KnownPassword = "Test password";

        private const string KnownSalt = "FzE68Eo3tmhJ//occ6O0LfJslzxZB+0/";
        private const string KnownKey = "5YvZaYs6AKDTNDWJECpQRm7TfdmfuAhD/4KlZXV/CZI=";
        private const int KnownIterations = 1337;
        private const string KnownInitializationVector = "M1UwDNen+sAgDyApDeaqKg==";
        private const string KnownCiphertext = "mMKDU0Dw5Wyirtij4hezCoAHHUoUJelJaEJCEeitExI=";

        protected abstract RandomNumberGeneratorBase CreateRandomNumberGenerator();
        protected abstract KeyGeneratorBase CreateKeyGenerator(); 
        protected abstract AesBase CreateAes();
        
        [Test]
        public void WhenEncryptingASecret_TheDecryptedShouldMatch()
        {
            
            // Given
            var rng = CreateRandomNumberGenerator();
            var keyGenerator = CreateKeyGenerator();
            var aes = CreateAes();

            // When
            var salt = new Salt(rng);
            var secretKey = keyGenerator.Generate(KnownPassword, salt, KnownIterations);
            var initializationVector = new InitializationVector(rng);
            var ciphertext = aes.Encrypt(KnownSecret, secretKey, initializationVector);
            var secret = aes.Decrypt(ciphertext, secretKey, initializationVector);

            // Then
            salt.Value.Length.Should().Be(AesBase.SaltSizeInBits / 8);
            secretKey.Value.Length.Should().Be(AesBase.KeySizeInBits / 8);
            initializationVector.Value.Length.Should().Be(AesBase.BlockSizeInBits / 8);
            ciphertext.ToBase64().Should().NotBe(KnownSecret);
            ciphertext.ToBase64().Length.Should().BeGreaterThan(KnownSecret.Length);
            secret.Should().Be(KnownSecret);

        }

        [Test]
        public void WhenDecryptingACiphertext_TheSecretShouldMatch()
        {

            // Given
            var keyGenerator = CreateKeyGenerator();
            var aes = CreateAes();

            // When
            var salt = new Salt(KnownSalt);
            var keyFromString = new Key(KnownKey);
            var initializationVector = new InitializationVector(KnownInitializationVector);
            var ciphertext = new Ciphertext(KnownCiphertext);
            var generatedKey = keyGenerator.Generate(KnownPassword, salt, KnownIterations);            
            var secret = aes.Decrypt(ciphertext, generatedKey, initializationVector);

            // Then
            initializationVector.Value.Length.Should().Be(AesBase.BlockSizeInBits / 8);
            ciphertext.ToBase64().Should().NotBe(KnownSecret);
            ciphertext.Value.Length.Should().BeGreaterThan(KnownSecret.Length);
            generatedKey.Value.Length.Should().Be(AesBase.KeySizeInBits / 8);
            generatedKey.Value.Should().BeEquivalentTo(keyFromString.Value);
            secret.Should().Be(KnownSecret);

        }

        [Test]
        public void WhenGeneratingAKeyWithDifferentIterations_TheKeysShouldDiffer()
        {

            // Given
            var rng = CreateRandomNumberGenerator();
            var keyGenerator = CreateKeyGenerator();

            // When
            var salt = new Salt(rng);
            var secretKeyFromString = new Key(KnownKey);
            var keyManyIterations = keyGenerator.Generate(KnownPassword, salt, KnownIterations);
            var keySingleIteration = keyGenerator.Generate(KnownPassword, salt, 1);

            // Then
            keyManyIterations.Value.Length.Should().Be(AesBase.KeySizeInBits / 8);
            keySingleIteration.Value.Length.Should().Be(AesBase.KeySizeInBits / 8);
            keyManyIterations.Value.Should().NotBeEquivalentTo(secretKeyFromString.Value);
            keySingleIteration.Value.Should().NotBeEquivalentTo(keyManyIterations.Value);

        }

        [Test]
        public void WhenEncryptingDataWithDifferentKeys_TheCiphertextShouldDiffer()
        {

            // Given
            var rng = CreateRandomNumberGenerator();
            var keyGenerator = CreateKeyGenerator();
            var aes = CreateAes();

            // When
            var salt = new Salt(rng);
            var initializationVector = new InitializationVector(rng);
            var keyManyIterations = keyGenerator.Generate(KnownPassword, salt, KnownIterations);
            var ciphertextManyIterations = aes.Encrypt(KnownSecret, keyManyIterations, initializationVector);
            var secretManyIterations = aes.Decrypt(ciphertextManyIterations, keyManyIterations, initializationVector);
            var keySingleIteration = keyGenerator.Generate(KnownPassword, salt, 1);
            var ciphertextSingleIteration = aes.Encrypt(KnownSecret, keySingleIteration, initializationVector);
            var secretSingleIteration = aes.Decrypt(ciphertextSingleIteration, keySingleIteration, initializationVector);

            // Then
            ciphertextManyIterations.Value.Should().NotBeEquivalentTo(ciphertextSingleIteration.Value);
            secretSingleIteration.Should().Be(secretManyIterations);
        }


        [Test]
        public void WhenDoubleEncryptingASecret_TheDecryptedShouldMatch()
        {

            // Given
            var rng = CreateRandomNumberGenerator();
            var keyGenerator = CreateKeyGenerator();
            var aes = CreateAes();

            // When
            var firstSalt = new Salt(rng);
            var firstKey = keyGenerator.Generate(KnownPassword, firstSalt, KnownIterations);
            var secondSalt = new Salt(rng);
            var secondKey = keyGenerator.Generate(firstKey, secondSalt, 1);
            var initializationVector = new InitializationVector(rng);
            var ciphertext = aes.Encrypt(KnownSecret, secondKey, initializationVector);
            var secret = aes.Decrypt(ciphertext, secondKey, initializationVector);

            // Then
            firstSalt.Value.Should().NotBeEquivalentTo(secondSalt.Value);
            firstKey.Value.Should().NotBeEquivalentTo(secondKey.Value);
            secret.Should().Be(KnownSecret);

        }

    }
}
