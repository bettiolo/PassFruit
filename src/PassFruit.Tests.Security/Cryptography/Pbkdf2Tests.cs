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
    public abstract class Pbkdf2Tests
    {

        private const string KnownPassword = "Secret Password";
        private const int KnownIterations = 1337;

        protected abstract Pbkdf2 CreatePbkdf2();

        [Test]
        public void WhenGeneratingKeysWithDifferentIterations_TheyShouldDiffer()
        {
            // Given
            var pbkdf2 = CreatePbkdf2();
            var manyIterations = 2000;
            var fewIterations = 1000;
            var salt = pbkdf2.GenerateSalt();

            // When
            var keyManyIterations = pbkdf2.Compute(KnownPassword, salt, manyIterations);
            var keyFewIterations = pbkdf2.Compute(KnownPassword, salt, fewIterations);

            // Then
            keyManyIterations.Length.Should().Be(Aes.KeySizeInBits / 8);
            keyFewIterations.Length.Should().Be(Aes.KeySizeInBits / 8);
            keyFewIterations.Should().NotBeEquivalentTo(keyManyIterations);
        }

        [Test]
        public void WhenGeneratingTwiceTheSameKey_TheyShouldBeTheSame()
        {
            // Given
            var firstPbkdf2 = CreatePbkdf2();
            var secondPbkdf2 = CreatePbkdf2();
            var salt = firstPbkdf2.GenerateSalt();

            // When
            var firstKey = firstPbkdf2.Compute(KnownPassword, salt, KnownIterations);
            var secondKey = secondPbkdf2.Compute(KnownPassword, salt, KnownIterations);

            // Then
            firstKey.Should().BeEquivalentTo(secondKey);
        }

        [Test]
        public void WhenGeneratingKeysFromDifferentPasswords_TheyShouldDiffer()
        {
            // Given
            var pbkdf2 = CreatePbkdf2();
            var salt = pbkdf2.GenerateSalt();

            // When
            var firstKey = pbkdf2.Compute(KnownPassword, salt, KnownIterations);
            var secondKey = pbkdf2.Compute("Different Password", salt, KnownIterations);

            // Then
            firstKey.Should().NotBeEquivalentTo(secondKey);
        }

        [Test]
        public void WhenGeneratingKeysWithTheSamePasswordButTwoDifferentSalts_TheyShouldDiffer()
        {
            // Given
            var pbkdf2 = CreatePbkdf2();
            var firstSalt = pbkdf2.GenerateSalt();
            var secondSalt = pbkdf2.GenerateSalt();
            
            // When
            var firstKey = pbkdf2.Compute(KnownPassword, firstSalt, KnownIterations);
            var secondKey = pbkdf2.Compute(KnownPassword, secondSalt, KnownIterations);

            // Then
            firstKey.Should().NotBeEquivalentTo(secondKey);
        }

        [Test]
        public void WhenGeneratingAKeyWithAnInvalidSalt_AnErrorShouldBeThrown()
        {
            // Given
            var pbkdf2 = CreatePbkdf2();
            var salt = pbkdf2.GenerateSalt();
            var shortSalt = new byte[32 / 8];
            var notLongEnoughSalt = new byte[120 / 8];
            var tooBigSalt = new byte[512 / 8];
            var emptySalt = new byte[0];
            Buffer.BlockCopy(salt, 0, shortSalt, 0, 32 / 8);
            Buffer.BlockCopy(salt, 0, notLongEnoughSalt, 0, 120 / 8);
            Buffer.BlockCopy(salt, 0, tooBigSalt, 0, salt.Length);

            // When
            Action keyFromShortSalt = () => pbkdf2.Compute(KnownPassword, shortSalt, KnownIterations);
            Action keyFromNotLongEnoughSalt = () => pbkdf2.Compute(KnownPassword, notLongEnoughSalt, KnownIterations);
            Action keyFromEmptySalt = () => pbkdf2.Compute(KnownPassword, emptySalt, KnownIterations);
            Action keyFromNullSalt = () => pbkdf2.Compute(KnownPassword, null, KnownIterations);
            Action keyFromTooBigSalt = () => pbkdf2.Compute(KnownPassword, tooBigSalt, KnownIterations);
            
            // Then
            keyFromShortSalt.ShouldThrow<ArgumentException>();
            keyFromNotLongEnoughSalt.ShouldThrow<ArgumentException>();
            keyFromEmptySalt.ShouldThrow<ArgumentException>();
            keyFromNullSalt.ShouldThrow<ArgumentException>();
            keyFromTooBigSalt.ShouldThrow<ArgumentException>();
        }

    }
}
