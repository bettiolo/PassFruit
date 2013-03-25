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

        protected abstract Authorizer CreateAuthorizer();
        protected abstract Encryptor CreateEncryptor();

        private const string KnownPassword = "Secret Password";
        private const string KnownMessage = "Message";
        private const int KnownDataIterations = 10;

        private static readonly byte[] KnownMasterSalt = AuthorizerTests.KnownAuthorization.Salt;
        private static readonly int KnownMasterIterations = AuthorizerTests.KnownAuthorization.Iterations;
        private static readonly EncryptedDataDto KnownEncryptedData = new EncryptedDataDto
        {
            Salt = Convert.FromBase64String("rHC/F6Ti0XJ2URX7wHyU9b6iJuO6jCqqM/EWnZlhbb4="),
            InitializationVector = Convert.FromBase64String("2bO/VA/xWARjbLGaiDuZ4g=="),
            Iterations = KnownDataIterations,
            Ciphertext = Convert.FromBase64String("ls13NxOa4Hla0IfhIX/x6w==")
        };

        [Test]
        public void WhenEncryptingData_TheMessageShouldBeDecryptedCorrectly()
        {
            // Given
            var authorizer = CreateAuthorizer();
            var encryptor = CreateEncryptor();

            // When
            var masterKey = authorizer.ComputeKey(KnownPassword, KnownMasterSalt, KnownMasterIterations);
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
            var authorizer = CreateAuthorizer();
            var encryptor = CreateEncryptor();

            // When
            var masterKey = authorizer.ComputeKey(KnownPassword, KnownMasterSalt, KnownMasterIterations);
            var message = encryptor.DecryptData(KnownEncryptedData, masterKey);

            // Then
            message.Should().Be(KnownMessage);
        }

        [Test]
        public void WhenEncryptingTheSameDataTwice_TheCiphertextShouldBeDifferentButTheMessageNot()
        {
            // Given
            var authorizer = CreateAuthorizer();
            var encryptor = CreateEncryptor();

            // When
            var masterKey = authorizer.ComputeKey(KnownPassword, KnownMasterSalt, KnownMasterIterations);
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

    }
}
