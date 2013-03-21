using System;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Security.Cryptography;

namespace PassFruit.Tests.Security.Cryptography
{
    [TestFixture]
    public abstract class AesTests
    {

        private const string KnownSecret = "Test of a secret message";
        private const string KnownPassword = "Test password";

        private const string KnownSalt = "FzE68Eo3tmhJ//occ6O0LfJslzxZB+0/";
        private const string KnownKey = "5YvZaYs6AKDTNDWJECpQRm7TfdmfuAhD/4KlZXV/CZI=";
        private const int KnownIterations = 1337;
        private const string KnownInitializationVector = "M1UwDNen+sAgDyApDeaqKg==";
        private const string KnownCiphertext = "mMKDU0Dw5Wyirtij4hezCoAHHUoUJelJaEJCEeitExI=";

        protected abstract Aes CreateAes();
        
        [Test]
        public void WhenEncryptingASecret_TheDecryptedShouldMatch()
        {
            
            // Given
            var aes = CreateAes();

            // When      
            var masterKey = aes.DeriveNewMasterKey(KnownPassword, KnownIterations);
            var dataCiphertext = aes.EncryptData(KnownSecret, masterKey);
            var secret = aes.DecryptData(dataCiphertext, masterKey);

            // Then
            masterKey.Iterations.Should().Be(KnownIterations);
            masterKey.Salt.Value.Length.Should().Be(Aes.SaltSizeInBits / 8);
            masterKey.Value.Length.Should().Be(Aes.KeySizeInBits / 8);
            dataCiphertext.Salt.Value.Length.Should().Be(Aes.SaltSizeInBits / 8);
            dataCiphertext.Salt.Value.Should().NotBeEquivalentTo(masterKey.Salt.Value);
            dataCiphertext.InitializationVector.Value.Length.Should().Be(Aes.BlockSizeInBits / 8);
            Convert.ToBase64String(dataCiphertext.Value).Should().NotBe(KnownSecret);
            Convert.ToBase64String(dataCiphertext.Value).Length.Should().BeGreaterThan(KnownSecret.Length);
            secret.Should().Be(KnownSecret);

        }

        [Test]
        public void WhenDecryptingACiphertext_TheSecretShouldMatch()
        {

            // Given
            var aes = CreateAes();

            // When
            var masterKey = aes.DeriveMasterKey(KnownPassword, KnownSalt, KnownIterations);
            var decryptedSecret = aes.DecryptMaster(KnownCiphertext, masterKey, KnownInitializationVector);

            // Then
            masterKey.Salt.Value.Length.Should().Be(Aes.SaltSizeInBits / 8);
            Convert.ToBase64String(masterKey.Salt.Value).Should().Be(KnownSalt);
            masterKey.Iterations.Should().Be(KnownIterations);
            masterKey.Value.Length.Should().Be(Aes.KeySizeInBits / 8);
            Convert.ToBase64String(masterKey.Value).Should().Be(KnownKey);
            decryptedSecret.Should().Be(KnownSecret);

        }

        [Test]
        public void WhenGeneratingAKeyWithDifferentIterations_TheKeysShouldDiffer()
        {

            // Given
            var aes = CreateAes();
            var manyIterations = 2000;
            var fewIterations = 1000;

            // When
            var keyManyIterations = aes.DeriveNewMasterKey(KnownPassword, manyIterations);
            var keySingleIteration = aes.DeriveNewMasterKey(KnownPassword, fewIterations);

            // Then
            keyManyIterations.Value.Length.Should().Be(Aes.KeySizeInBits / 8);
            keySingleIteration.Value.Length.Should().Be(Aes.KeySizeInBits / 8);
            keySingleIteration.Value.Should().NotBeEquivalentTo(keyManyIterations.Value);

        }

        [Test]
        public void WhenEncryptingWithDifferentKeys_TheCiphertextShouldDiffer()
        {

            // Given
            var aes = CreateAes();
            var manyIterations = 2000;
            var fewIterations = 1000;

            // When
            var keyManyIterations = aes.DeriveNewMasterKey(KnownPassword, manyIterations);
            var ciphertextManyIterations = aes.EncryptMaster(KnownSecret, keyManyIterations);
            var secretManyIterations = aes.DecryptMaster(ciphertextManyIterations, keyManyIterations);
            var keyFewIterations = aes.DeriveNewMasterKey(KnownPassword, fewIterations);
            var ciphertextFewIterations = aes.EncryptMaster(KnownSecret, keyFewIterations);
            var secretFewIterations = aes.DecryptMaster(ciphertextFewIterations, keyFewIterations);

            // Then
            keyManyIterations.Iterations.Should().Be(manyIterations);
            keyFewIterations.Iterations.Should().Be(fewIterations);
            ciphertextManyIterations.Value.Should().NotBeEquivalentTo(ciphertextFewIterations.Value);
            secretManyIterations.Should().Be(KnownSecret);
            secretFewIterations.Should().Be(KnownSecret);
            secretFewIterations.Should().Be(secretManyIterations);
        }

    }
}
