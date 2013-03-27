using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Security;

namespace PassFruit.Tests.Security
{
    [TestFixture]
    public abstract class EncryptorTests
    {

        private const string KnownPassword = "Secret Password";
        private const string KnownMessage = "Message";
        private const int KnownMasterIterations = 1337;
        private const int KnownDataIterations = 10;

        private static readonly byte[] KnownMasterSalt = Convert.FromBase64String("itYhNYNwDwGT3LBeKsS7Ql4fEKN0oiml1QUokr9952Y=");
        private static readonly byte[] KnownDataSalt = Convert.FromBase64String("rHC/F6Ti0XJ2URX7wHyU9b6iJuO6jCqqM/EWnZlhbb4=");
        private static readonly byte[] KnownDataInitializationVector = Convert.FromBase64String("2bO/VA/xWARjbLGaiDuZ4g==");
        private static readonly byte[] KnownCiphertext = Convert.FromBase64String("ls13NxOa4Hla0IfhIX/x6w==");

        private static readonly EncryptedData KnownEncryptedData = 
            new EncryptedData(KnownDataSalt, KnownDataInitializationVector, KnownDataIterations, KnownCiphertext);

        protected abstract MasterKey ComputeMasterKey(string secretPassword, byte[] salt, int iterations);
        protected abstract Encryptor CreateEncryptor();

        [Test]
        public void WhenEncryptingData_TheMessageShouldBeDecryptedCorrectly()
        {
            // Given
            var masterKey = ComputeMasterKey(KnownPassword, KnownMasterSalt, KnownMasterIterations).SecretKey;
            var encryptor = CreateEncryptor();

            // When
            var encryptedData = encryptor.EncryptData(KnownMessage, masterKey, KnownDataIterations);
            var message = encryptor.DecryptData(encryptedData, masterKey);

            // Then
            encryptedData.Salt.Should().NotBeEquivalentTo(KnownMasterSalt);
            encryptedData.Iterations.Should().Be(KnownDataIterations);
            encryptedData.Iterations.Should().NotBe(KnownMasterIterations);
            message.Should().Be(KnownMessage);
        }

        [Test]
        public void WhenDecryptingData_TheMessageShouldBeDecryptedCorrectly()
        {
            // Given
            var masterKey = ComputeMasterKey(KnownPassword, KnownMasterSalt, KnownMasterIterations).SecretKey;
            var encryptor = CreateEncryptor();

            // When
            var message = encryptor.DecryptData(KnownEncryptedData, masterKey);

            // Then
            message.Should().Be(KnownMessage);
        }

        [Test]
        public void WhenEncryptingTheSameDataTwice_TheCiphertextShouldBeDifferentButTheMessageNot()
        {
            // Given
            var masterKey = ComputeMasterKey(KnownPassword, KnownMasterSalt, KnownMasterIterations).SecretKey;
            var encryptor = CreateEncryptor();

            // When
            var firstEncryptedData = encryptor.EncryptData(KnownMessage, masterKey, KnownDataIterations);
            var fistMessage = encryptor.DecryptData(firstEncryptedData, masterKey);
            var secondEncryptedData = encryptor.EncryptData(KnownMessage, masterKey, KnownDataIterations);
            var secondMessage = encryptor.DecryptData(secondEncryptedData, masterKey);

            // Then
            firstEncryptedData.Salt.Should().NotBeEquivalentTo(secondEncryptedData.Salt);
            firstEncryptedData.Iterations.Should().Be(secondEncryptedData.Iterations);
            firstEncryptedData.InitializationVector.Should().NotBeEquivalentTo(secondEncryptedData.InitializationVector);
            firstEncryptedData.Ciphertext.Should().NotBeEquivalentTo(secondEncryptedData.Ciphertext);
            fistMessage.Should().Be(secondMessage);
            fistMessage.Should().Be(KnownMessage);
            secondMessage.Should().Be(KnownMessage);
        }

        // ToDo: Create a test with the wrong key or password

    }
}
