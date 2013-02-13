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
    public class EncryptionTests
    {

        private const string KnownSecretMessage = "Test of a secret message";
        private const string KnownSecretPassword = "Test password";

        [Test]
        public void WhenEncryptingAMessage_TheDecryptedShouldMatch()
        {
            var aes = new Aes();

            var salt = aes.GenerateRandomSalt();
            salt.Length.Should().Be(Aes.SaltSizeInBits / 8);

            var secretKey = aes.GenerateKey(KnownSecretPassword, salt);
            secretKey.Length.Should().Be(Aes.KeySizeInBits / 8);

            var initializationVector = aes.GenerateRandomInitializationVector();
            initializationVector.Length.Should().Be(Aes.BlockSizeInBits / 8);

            var base64Ciphertext = aes.EncryptToBase64(KnownSecretMessage, secretKey, initializationVector);
            base64Ciphertext.Should().NotBe(KnownSecretMessage);
            base64Ciphertext.Length.Should().BeGreaterThan(KnownSecretMessage.Length);

            var decryptedSecretMessage = aes.DecryptFromBase64(base64Ciphertext, secretKey, initializationVector);
            decryptedSecretMessage.Should().Be(KnownSecretMessage);
        }

        [Test]
        public void WhenDecryptingACiphertext_TheDecryptedShouldBeCorrect()
        {
            var ciphertext = "mMKDU0Dw5Wyirtij4hezCoAHHUoUJelJaEJCEeitExI=";
            var salt = Convert.FromBase64String("FzE68Eo3tmhJ//occ6O0LfJslzxZB+0/");
            var initializationVector = Convert.FromBase64String("M1UwDNen+sAgDyApDeaqKg==");

            var aes = new Aes();

            var secretKeyFromString = Convert.FromBase64String("5YvZaYs6AKDTNDWJECpQRm7TfdmfuAhD/4KlZXV/CZI=");
            var secretKey = aes.GenerateKey(KnownSecretPassword, salt);
            secretKey.Length.Should().Be(Aes.KeySizeInBits / 8);
            secretKey.Should().BeEquivalentTo(secretKeyFromString);

            initializationVector.Length.Should().Be(Aes.BlockSizeInBits / 8);

            ciphertext.Should().NotBe(KnownSecretMessage);
            ciphertext.Length.Should().BeGreaterThan(KnownSecretMessage.Length);

            var decryptedSecretMessage = aes.DecryptFromBase64(ciphertext, secretKey, initializationVector);
            decryptedSecretMessage.Should().Be(KnownSecretMessage);
        }

    }
}
