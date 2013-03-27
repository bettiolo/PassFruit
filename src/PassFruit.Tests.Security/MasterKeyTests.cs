using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Security;
using PassFruit.Security.Cryptography;

namespace PassFruit.Tests.Security
{
    [TestFixture]
    public abstract class MasterKeyTests
    {

        private const string KnownPassword = "Secret Password";
        private const int KnownIterations = 1337;

        private static readonly byte[] KnownSalt = Convert.FromBase64String("itYhNYNwDwGT3LBeKsS7Ql4fEKN0oiml1QUokr9952Y=");
        private static readonly byte[] KnownSecretKey = Convert.FromBase64String("35gSi/PfRb2dG/5/jAy0z4qp0e87tIaWv3lLF62a39E=");

        protected abstract Pbkdf2 CreatePbkdf2();

        [Test]
        public void WhenCreatingANewMasterKey_ACorrectOneShouldBeCreated()
        {
            // Given
            var pbkdf2 = CreatePbkdf2();

            // When
            var masterKey = new MasterKey(KnownPassword, KnownIterations, pbkdf2);

            // Then
            masterKey.Iterations.Should().Be(KnownIterations);
            masterKey.Salt.Length.Should().Be(Pbkdf2.SaltSizeInBits / 8);
            masterKey.Salt.Should().NotBeEmpty();
            masterKey.SecretKey.Should().NotBeEmpty();
        }

        [Test]
        public void WhenCreatingAnExistingMasterKey_ACorrectOneShouldBeCreated()
        {
            // Given
            var pbkdf2 = CreatePbkdf2();

            // When
            var masterKey = new MasterKey(KnownPassword, KnownSalt, KnownIterations, pbkdf2);

            // Then
            masterKey.SecretKey.Should().BeEquivalentTo(KnownSecretKey);
        }

        [Test]
        public void WhenCreatingAnExistingMasterKeyWithAWrongPassword_ABadOneShouldBeCreated()
        {
            // Given
            var pbkdf2 = CreatePbkdf2();

            // When
            var masterKey = new MasterKey("Wrong Password", KnownSalt, KnownIterations, pbkdf2);

            // Then
            masterKey.SecretKey.Should().NotBeEquivalentTo(KnownSecretKey);
        }

        // ToDo: Think about implementing password verification and relative tests

    }
}
