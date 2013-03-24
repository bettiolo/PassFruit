using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Security.Cryptography;

namespace PassFruit.Tests.Security.Cryptography
{
    [TestFixture]
    public abstract class HmacSha256Tests
    {

        private static readonly byte[] KnownPassword = Encoding.UTF8.GetBytes("Secret Password");
        private static readonly byte[] KnownMessage = Encoding.UTF8.GetBytes("Message");

        protected abstract HmacSha256 CreateHmacSha256();

        [Test]
        public void WhenHashingTheSameMessageAndPassword_SameHashShouldBeComputed()
        {
            // Given
            var fistHmacSha256 = CreateHmacSha256();
            var secondHmacSha256 = CreateHmacSha256();

            // When
            var firstHash = fistHmacSha256.Compute(KnownMessage, KnownPassword);
            var secondHash = secondHmacSha256.Compute(KnownMessage, KnownPassword);

            // Then
            firstHash.Length.Should().Be(HmacSha256.HmacSizeInBits / 8);
            firstHash.Should().NotBeEmpty();
            secondHash.Length.Should().Be(HmacSha256.HmacSizeInBits / 8);
            secondHash.Should().NotBeEmpty();
            firstHash.Should().BeEquivalentTo(secondHash);
        }

        [Test]
        public void WhenHashingDifferentMessageOrPassword_DifferentHashShouldBeComputed()
        {
            // Given
            var hmacSha256 = CreateHmacSha256();
            var differentMessage = Encoding.UTF8.GetBytes("Different Message");
            var differentPassword = Encoding.UTF8.GetBytes("Different Password");

            // When
            var knownHash = hmacSha256.Compute(KnownMessage, KnownPassword);
            var differentMessageHash = hmacSha256.Compute(differentMessage, KnownPassword);
            var differentPasswordHash = hmacSha256.Compute(KnownMessage, differentPassword);

            // Then
            knownHash.Should().NotBeEquivalentTo(differentMessageHash);
            knownHash.Should().NotBeEquivalentTo(differentPasswordHash);
            differentMessageHash.Should().NotBeEquivalentTo(differentPasswordHash);
        }

    }
}
