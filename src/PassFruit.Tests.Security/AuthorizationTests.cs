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

        public static readonly AuthorizationDto KnownAuthorization = new AuthorizationDto
        {
            Iterations = KnownIterations,
            Salt = Convert.FromBase64String("itYhNYNwDwGT3LBeKsS7Ql4fEKN0oiml1QUokr9952Y="),
            InitializationVector = Convert.FromBase64String("Lavvbjg/ALBMA823uCxTxA=="),
            Hmac = Convert.FromBase64String("lOaje8/j/zVfCS+WDxajn07pzAC2rv+6Hp1zocZtO9s=")
        };
        private static readonly byte[] KnownSalt = KnownAuthorization.Salt;

        protected abstract Authorizer CreateAuthorizer();

        [Test]
        public void WhenCreatingANewAuthorization_ItShouldBeCreated()
        {
            // Given
            var authorizer = CreateAuthorizer();

            // When
            var key = authorizer.ComputeKey(KnownPassword, KnownSalt, KnownIterations);
            var authorization = authorizer.CreateAuthorization(key, KnownIterations);

            // Then
            authorization.Iterations.Should().Be(KnownIterations);
            authorization.Salt.Length.Should().Be(Pbkdf2.SaltSizeInBits / 8);
            authorization.Salt.Should().NotBeEmpty();
            authorization.InitializationVector.Length.Should().Be(Aes.BlockSizeInBits / 8);
            authorization.InitializationVector.Should().NotBeEmpty();
            authorization.Hmac.Length.Should().Be(HmacSha256.HmacSizeInBits / 8);
            authorization.Hmac.Should().NotBeEmpty();
        }

        [Test]
        public void WhenCreatingTheSameHmacTwice_ADifferentHashShouldBeGeneratedBecauseOfTheInitializationVector()
        {
            // Given
            var authorizer = CreateAuthorizer();

            // When
            var key = authorizer.ComputeKey(KnownPassword, KnownSalt, KnownIterations);
            var firstHmac =  authorizer.CreateAuthorization(key, KnownIterations).Hmac;
            var secondHmac = authorizer.CreateAuthorization(key, KnownIterations).Hmac;
            
            // Then
            firstHmac.Should().NotBeEquivalentTo(secondHmac);
        }

        [Test]
        public void WhenAuthorizingJustCreatedAuthorization_ItShouldBeAuthorized()
        {
            // Given
            var firstAuthorizer = CreateAuthorizer();
            var secondAuthorizer = CreateAuthorizer();

            // When
            var key = firstAuthorizer.ComputeKey(KnownPassword, KnownSalt, KnownIterations);
            var authorization = firstAuthorizer.CreateAuthorization(key, KnownIterations);
            var authorized = secondAuthorizer.Authorize(key, authorization);

            // Then
            authorized.Should().BeTrue();
        }

        [Test]
        public void WhenAuthorizingTheCorrectPassword_ItShouldBeAuthorized()
        {
            // Given
            var authorizer = CreateAuthorizer();

            // When
            var key = authorizer.ComputeKey(KnownPassword, KnownSalt, KnownIterations);
            var authorized = authorizer.Authorize(key, KnownAuthorization);

            // Then
            authorized.Should().BeTrue();
        }

        [Test]
        public void WhenAuthorizingTheWrongPassword_ItShouldNotBeAuthorized()
        {
            // Given
            var authorizer = CreateAuthorizer();
            var salt = KnownAuthorization.Salt;

            // When
            var key = authorizer.ComputeKey("Different Password", salt, KnownIterations);
            var authorized = authorizer.Authorize(key, KnownAuthorization);

            // Then
            authorized.Should().BeFalse();
        }

    }
}
