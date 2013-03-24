using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Security.Cryptography;

namespace PassFruit.Tests.Security.Cryptography
{
    [TestFixture]
    public abstract class AesTests
    {

        private const string KnownPassword = "Secret Password";
        private const string KnownMessage = "Message";
        private const int KnownIterations = 1337;
        private static readonly byte[] KnownSalt = Convert.FromBase64String("R++DuFZS+JvBaN9Rv9nNK49u9s6AflNxCY/u6R4/3KE=");
        private static readonly byte[] KnownKey = Convert.FromBase64String("7EiKmzn3Yl6Q5WzKunu7HM5jg6/yMhQfrKpgRncXz6c=");
        private static readonly byte[] KnownInitializationVector = Convert.FromBase64String("cIgJOngkt2azZrFmSufqYw==");
        private static readonly byte[] KnownCiphertext = Convert.FromBase64String("8WlIeXCofXfhtRRghoVUog==");

        protected abstract Aes CreateAes();
        protected abstract Pbkdf2 CreatePbkdf2();
        
        [Test]
        public void WhenEncryptingASecret_TheDecryptedShouldMatch()
        {
            // Given
            var aes = CreateAes();
            var pbkdf2 = CreatePbkdf2();
            var salt = pbkdf2.GenerateSalt();
            var initializationVector = aes.GenerateInitializationVector();

            // When            
            var key = pbkdf2.Compute(KnownPassword, salt, KnownIterations);
            var ciphertext = aes.Encrypt(KnownMessage, key, initializationVector);
            var message = aes.Decrypt(ciphertext, key, initializationVector);

            // Then
            salt.Length.Should().Be(Pbkdf2.SaltSizeInBits / 8);
            key.Length.Should().Be(Aes.KeySizeInBits / 8);
            initializationVector.Length.Should().Be(Aes.BlockSizeInBits / 8);
            ciphertext.Should().NotBeEquivalentTo(Encoding.UTF8.GetBytes(KnownMessage));
            ciphertext.Length.Should().BeGreaterThan(KnownMessage.Length);
            message.Should().Be(KnownMessage);
        }

        [Test]
        public void WhenDecryptingACiphertextWithTheCorrectPassword_TheSecretShouldMatch()
        {
            // Given
            var aes = CreateAes();
            var pbkdf2 = CreatePbkdf2();

            // When
            var key = pbkdf2.Compute(KnownPassword, KnownSalt, KnownIterations);
            var message = aes.Decrypt(KnownCiphertext, key, KnownInitializationVector);

            // Then
            key.Should().BeEquivalentTo(KnownKey);
            message.Should().Be(KnownMessage);
        }

        [Test]
        public void WhenEncryptingWithDifferentSalts_TheCiphertextShouldDiffer()
        {

            // Given
            var aes = CreateAes();
            var pbkdf2 = CreatePbkdf2();
            var firstSalt = pbkdf2.GenerateSalt();
            var secondSalt = pbkdf2.GenerateSalt();
            var initializationVector = aes.GenerateInitializationVector();

            // When
            var firstKey = pbkdf2.Compute(KnownPassword, firstSalt, KnownIterations);
            var firstCiphertext = aes.Encrypt(KnownMessage, firstKey, initializationVector);
            var firstMessage = aes.Decrypt(firstCiphertext, firstKey, initializationVector);
            var secondKey = pbkdf2.Compute(KnownPassword, secondSalt, KnownIterations);
            var secondCiphertext = aes.Encrypt(KnownMessage, secondKey, initializationVector);
            var secondMessage = aes.Decrypt(secondCiphertext, secondKey, initializationVector);

            // Then
            firstKey.Should().NotBeEquivalentTo(secondKey);
            firstCiphertext.Should().NotBeEquivalentTo(secondCiphertext);
            firstMessage.Should().BeEquivalentTo(secondMessage);
            firstMessage.Should().Be(KnownMessage);
            secondMessage.Should().Be(KnownMessage);
        }

        [Test]
        public void WhenEncryptingWithDifferentPasswords_TheCiphertextShouldDiffer()
        {
            // Given
            var aes = CreateAes();
            var pbkdf2 = CreatePbkdf2();
            var salt = pbkdf2.GenerateSalt();
            var initializationVector = aes.GenerateInitializationVector();

            // When
            var firstKey = pbkdf2.Compute(KnownPassword, salt, KnownIterations);
            var firstCiphertext = aes.Encrypt(KnownMessage, firstKey, initializationVector);
            var firstMessage = aes.Decrypt(firstCiphertext, firstKey, initializationVector);
            var secondKey = pbkdf2.Compute("Different Password", salt, KnownIterations);
            var secondCiphertext = aes.Encrypt(KnownMessage, secondKey, initializationVector);
            var secondMessage = aes.Decrypt(secondCiphertext, secondKey, initializationVector);

            // Then
            firstKey.Should().NotBeEquivalentTo(secondKey);
            firstCiphertext.Should().NotBeEquivalentTo(secondCiphertext);
            firstMessage.Should().BeEquivalentTo(secondMessage);
            firstMessage.Should().Be(KnownMessage);
            secondMessage.Should().Be(KnownMessage);
        }

        [Test]
        public void WhenEncryptingWithDifferentInitializationVectors_TheCiphertextShouldDiffer()
        {
            // Given
            var aes = CreateAes();
            var pbkdf2 = CreatePbkdf2();
            var salt = pbkdf2.GenerateSalt();
            var firstInitializationVector = aes.GenerateInitializationVector();
            var secondInitializationVector = aes.GenerateInitializationVector();

            // When
            var key = pbkdf2.Compute(KnownPassword, salt, KnownIterations);
            var firstCiphertext = aes.Encrypt(KnownMessage, key, firstInitializationVector);
            var firstMessage = aes.Decrypt(firstCiphertext, key, firstInitializationVector);
            var secondCiphertext = aes.Encrypt(KnownMessage, key, secondInitializationVector);
            var secondMessage = aes.Decrypt(secondCiphertext, key, secondInitializationVector);

            // Then
            firstInitializationVector.Should().NotBeEquivalentTo(secondInitializationVector);
            firstCiphertext.Should().NotBeEquivalentTo(secondCiphertext);
            firstMessage.Should().BeEquivalentTo(secondMessage);
            firstMessage.Should().Be(KnownMessage);
            secondMessage.Should().Be(KnownMessage);
        }

    }
}
