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

        private AuthorizationDto KnownAuthorizationDto = new AuthorizationDto
        {
            Iterations = KnownIterations,
            Salt = Convert.FromBase64String("itYhNYNwDwGT3LBeKsS7Ql4fEKN0oiml1QUokr9952Y="),
            InitializationVector = Convert.FromBase64String("Lavvbjg/ALBMA823uCxTxA=="),
            Hmac = Convert.FromBase64String("lOaje8/j/zVfCS+WDxajn07pzAC2rv+6Hp1zocZtO9s=")
        };

        protected abstract Authorizer CreateAuthorizer();

        [Test]
        public void WhenCreatingANewAuthorization_ItShouldBeCreated()
        {
            // Given
            var authorizer = CreateAuthorizer();

            // When
            var authorization = authorizer.CreateAuthorization(KnownPassword, KnownIterations);

            // Then
            authorization.Iterations.Should().Be(KnownIterations);
            authorization.Salt.Length.Should().Be(Pbkdf2.SaltSizeInBits / 8);
            authorization.Salt.Should().NotBeEmpty();
            authorization.InitializationVector.Length.Should().Be(Aes.BlockSizeInBits / 8);
            authorization.InitializationVector.Should().NotBeEmpty();
            authorization.Hmac.Length.Should().Be(HmacSha256.HmacSizeInBits / 8);
            authorization.Hmac.Should().NotBeEmpty();
        }

        private byte[] CreateAuthorizationHmac(Authorizer authorizer, string password)
        {

            return authorizer.CreateAuthorizationHmac(password, KnownAuthorizationDto.Salt,
                                                                KnownAuthorizationDto.Iterations,
                                                                KnownAuthorizationDto.InitializationVector);
        }

        [Test]
        public void WhenCreatingTheSameHmacTwice_TheSameHashShouldBeGenerated()
        {
            // Given
            var authorizer = CreateAuthorizer();

            // When
            var firstHmac = CreateAuthorizationHmac(authorizer, KnownPassword);
            var secondHmac = CreateAuthorizationHmac(authorizer, KnownPassword);
            
            // Then
            firstHmac.Should().BeEquivalentTo(secondHmac);
        }

        [Test]
        public void WhenTwoHmacsWithDifferentPassword_TheHashShouldBeDifferent()
        {
            // Given
            var authorizer = CreateAuthorizer();

            // When
            var firstHmac = CreateAuthorizationHmac(authorizer, KnownPassword);
            var secondHmac = CreateAuthorizationHmac(authorizer, "Different Password");

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
            var authorization = firstAuthorizer.CreateAuthorization(KnownPassword, KnownIterations);
            var authorized = secondAuthorizer.Authorize(KnownPassword, authorization);

            // Then
            authorized.Should().BeTrue();
        }

        [Test]
        public void WhenAuthorizingTheCorrectPassword_ShouldBeAuthorized()
        {
            // Given
            var authorizer = CreateAuthorizer();
   
            // When
            var authorized = authorizer.Authorize(KnownPassword, KnownAuthorizationDto);

            // Then
            authorized.Should().BeTrue();
        }

        [Test]
        public void WhenAuthorizingTheWrongPassword_ShouldNotBeAuthorized()
        {
            // Given
            var authorizer = CreateAuthorizer();

            // When
            var authorized = authorizer.Authorize("Bad Password", KnownAuthorizationDto);

            // Then
            authorized.Should().BeFalse();
        }

    }
}
