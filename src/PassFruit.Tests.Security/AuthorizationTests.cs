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
    public abstract class AuthorizationTests
    {

        private const string KnownPassword = "Secret Password";
        private const int KnownIterations = 1337;

        private static readonly byte[] KnownSalt = Convert.FromBase64String("itYhNYNwDwGT3LBeKsS7Ql4fEKN0oiml1QUokr9952Y=");
        private static readonly byte[] KnownInitializationVector = Convert.FromBase64String("Lavvbjg/ALBMA823uCxTxA==");
        private static readonly byte[] KnownHmac = Convert.FromBase64String("lOaje8/j/zVfCS+WDxajn07pzAC2rv+6Hp1zocZtO9s=");

        protected abstract Pbkdf2 CreatePbkdf2();
        protected abstract Aes CreateAes();
        protected abstract HmacSha256 CreateHmacSha256();

        [Test]
        public void WhenCreatingANewAuthorization_ItShouldBeCreated()
        {
            // Given
            var newMasterKey = new MasterKey(KnownPassword, KnownIterations, CreatePbkdf2());

            // When
            var newAuthorization = new Authorization(newMasterKey, CreateAes(), CreateHmacSha256());

            // Then
            newAuthorization.InitializationVector.Length.Should().Be(Aes.BlockSizeInBits / 8);
            newAuthorization.InitializationVector.Should().NotBeEmpty();
            newAuthorization.Hmac.Length.Should().Be(HmacSha256.HmacSizeInBits / 8);
            newAuthorization.Hmac.Should().NotBeEmpty();
        }

        [Test]
        public void WhenCreatingANewAuthorizationTwiceWithTheSameMasterKey_ADifferentHashShouldBeGeneratedBecauseTheInitializationVectorIsDifferent()
        {
            // Given
            var newMasterKey = new MasterKey(KnownPassword, KnownIterations, CreatePbkdf2());

            // When
            var firstNewAuthorization = new Authorization(newMasterKey, CreateAes(), CreateHmacSha256());
            var secondNewAuthorization = new Authorization(newMasterKey, CreateAes(), CreateHmacSha256());
            
            // Then
            firstNewAuthorization.Hmac.Should().NotBeEquivalentTo(secondNewAuthorization.Hmac);
        }

        [Test]
        public void WhenCreatingTwiceTheAuthorizationWithTheSameInitializationVector_ItShouldBeTheSame()
        {
            // Given
            var newMasterKey = new MasterKey(KnownPassword, KnownIterations, CreatePbkdf2());

            // When
            var newAuthorization = new Authorization(newMasterKey, CreateAes(), CreateHmacSha256());
            var existingAuthorization = new Authorization(newMasterKey, newAuthorization.InitializationVector,
                CreateAes(), CreateHmacSha256());

            // Then
            newAuthorization.Hmac.Should().BeEquivalentTo(existingAuthorization.Hmac);
        }

        [Test]
        public void WhenAuthorizingAnExistingCorrectMasterKey_TheCorrectHmacShouldBeComputer()
        {
            // Given
            var existingMasterKey = new MasterKey(KnownPassword, KnownSalt, KnownIterations, CreatePbkdf2());

            // When
            var existingAuthorization = new Authorization(existingMasterKey, KnownInitializationVector, CreateAes(), CreateHmacSha256());

            // Then
            existingAuthorization.InitializationVector.Should().BeEquivalentTo(KnownInitializationVector);
            existingAuthorization.Hmac.Should().BeEquivalentTo(KnownHmac);
        }

        [Test]
        public void WhenAuthorizingAnExistingWrongMasterKey_ADifferentHmacShouldBeComputed()
        {
            // Given
            var existingMasterKey = new MasterKey("Wrong Password", KnownSalt, KnownIterations, CreatePbkdf2());

            // When
            var existingAuthorization = new Authorization(existingMasterKey, KnownInitializationVector, CreateAes(), CreateHmacSha256());

            // Then
            existingAuthorization.Hmac.Should().NotBeEquivalentTo(KnownHmac);
        }

    }
}
