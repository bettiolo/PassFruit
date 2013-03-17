using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Server.FakeDataGenerator;

namespace PassFruit.Server.CiphertextDatastore.Tests
{

    [TestFixture]
    public abstract class CiphertextDatastoreTestBase
    {

        protected abstract CiphertextDatastoreBase CreateEmpty();

        protected abstract CiphertextDatastoreBase CreatePopulatedWithFakeData();

        protected abstract CiphertextDatastoreBase CreateReloadedPopulatedWithFakeData();

        private void VerifyThatStoredDataMatches(CiphertextDatastoreBase ciphertextDatastore,
                                                             CiphertextDto CiphertextDto)
        {
            var accountId = CiphertextDto.Id;
            ciphertextDatastore.GetAllIds()
                               .Should().Contain(id => id.Equals(accountId));

            ciphertextDatastore.GetAll()
                               .Should().Contain(datastoreCiphertextDto => accountId == datastoreCiphertextDto.Id);

            ciphertextDatastore.Get(accountId).Equals(CiphertextDto)
                               .Should().BeTrue();

            ciphertextDatastore.Get(accountId).Deleted
                               .Should().Be(CiphertextDto.Deleted);

            ciphertextDatastore.Get(accountId).Id
                               .Should().Be(CiphertextDto.Id);

            ciphertextDatastore.Get(accountId).Ciphertext
                               .Should().Equal(CiphertextDto.Ciphertext);

            ciphertextDatastore.Get(accountId).InitializationVector
                               .Should().Equal(CiphertextDto.InitializationVector);

            ciphertextDatastore.Get(accountId).Salt
                               .Should().Equal(CiphertextDto.Salt);
        }

        private void WhenAddingASingleAccountToADataStore_ItShouldBeAdded(CiphertextDatastoreBase ciphertextDatastore, int expectedCount)
        {

            // Given
            var fakeCiphertextDto = CiphertextFakeDataGenerator.GetFakeCiphertextDtoNotYetAdded();

            // When
            ciphertextDatastore.Save(fakeCiphertextDto);

            // Then
            ciphertextDatastore.GetAllIds()
                               .Should().HaveCount(expectedCount);

            ciphertextDatastore.GetAll(CiphertextStatus.Any)
                               .Should().HaveCount(expectedCount);

            ciphertextDatastore.GetAll(CiphertextStatus.Active)
                               .Should().HaveCount(expectedCount);

            ciphertextDatastore.GetAll(CiphertextStatus.Deleted)
                               .Should().HaveCount(0);

            VerifyThatStoredDataMatches(ciphertextDatastore, fakeCiphertextDto);
        }

        [Test]
        public void WhenAddingASingleAccountToAnEmptyDataStore_ItShouldBeAdded()
        {
            WhenAddingASingleAccountToADataStore_ItShouldBeAdded(
                CreateEmpty(),
                1);
        }

        [Test]
        public void WhenAddingASingleAccountToAPopulatedDataStore_ItShouldBeAdded()
        {
            WhenAddingASingleAccountToADataStore_ItShouldBeAdded(
                CreatePopulatedWithFakeData(),
                CiphertextFakeDataGenerator.PopulatedAccounts.Length + 1);
        }

        [Test]
        public void WhenAddingASingleAccountToAReloadedPopulatedDataStore_ItShouldBeAdded()
        {
            WhenAddingASingleAccountToADataStore_ItShouldBeAdded(
                CreateReloadedPopulatedWithFakeData(),
                CiphertextFakeDataGenerator.PopulatedAccounts.Length + 1);
        }

        [Test]
        public void WhenAddingMultipeAccounts_TheyShouldBeAdded()
        {

            // Given
            var fakeCiphertextDto1 = CiphertextFakeDataGenerator.GetFakeCiphertextDto1();
            var fakeCiphertextDto2 = CiphertextFakeDataGenerator.GetFakeCiphertextDto2();
            var fakeCiphertextDtoNotYetAdded =
                CiphertextFakeDataGenerator.GetFakeCiphertextDtoNotYetAdded();

            var ciphertextDatastore = CreateEmpty();

            // When
            ciphertextDatastore.Save(fakeCiphertextDto1);
            ciphertextDatastore.Save(fakeCiphertextDto2);

            // Then
            fakeCiphertextDtoNotYetAdded.Id
                   .Should().BeEmpty();

            ciphertextDatastore.GetAll(CiphertextStatus.Any)
                               .Should().HaveCount(2);

            ciphertextDatastore.GetAll(CiphertextStatus.Deleted)
                               .Should().HaveCount(0);

            ciphertextDatastore.GetAll(CiphertextStatus.Active)
                               .Should().HaveCount(2);

            VerifyThatStoredDataMatches(ciphertextDatastore, fakeCiphertextDto1);
            VerifyThatStoredDataMatches(ciphertextDatastore, fakeCiphertextDto2);
        }

        [Test]
        public void WhenDeletingAnAccountThatHasBeenAddedToAnEmptyDatastore_ItShouldBeDeleted()
        {

            // Given
            var fakeCiphertextDto1 = CiphertextFakeDataGenerator.GetFakeCiphertextDto1();

            var ciphertextDatastore = CreateEmpty();

            // When
            ciphertextDatastore.Save(fakeCiphertextDto1);
            ciphertextDatastore.Delete(fakeCiphertextDto1.Id);

            // Then
            ciphertextDatastore.GetAll(CiphertextStatus.Active)
                               .Should().NotContain(account => account.Id == fakeCiphertextDto1.Id);

            ciphertextDatastore.GetAll(CiphertextStatus.Deleted)
                               .Should().Contain(account => account.Id == fakeCiphertextDto1.Id);

            ciphertextDatastore.GetAllIds()
                               .Should().HaveCount(1); // Because AllIds return also deleted ones

            ciphertextDatastore.GetAll(CiphertextStatus.Active).Count()
                               .Should().BeLessThan(ciphertextDatastore.GetAll(CiphertextStatus.Any).Count());

            ciphertextDatastore.Get(fakeCiphertextDto1.Id).Id
                               .Should().NotBeEmpty();

            ciphertextDatastore.Get(fakeCiphertextDto1.Id).Ciphertext
                               .Should().BeEmpty();

            ciphertextDatastore.Get(fakeCiphertextDto1.Id).Deleted
                               .Should().BeTrue();

            ciphertextDatastore.Get(fakeCiphertextDto1.Id).InitializationVector
                               .Should().BeEmpty();

            ciphertextDatastore.Get(fakeCiphertextDto1.Id).Salt
                               .Should().BeEmpty();

            ciphertextDatastore.Get(fakeCiphertextDto1.Id).IsNew()
                               .Should().BeFalse();

        }

        [Test]
        public void WhenDeletingAnAccountThatHasBeenAddedToAPopulatedDatastore_ItShouldBeDeleted()
        {

            // Given
            var ciphertextDatastore = CreatePopulatedWithFakeData();
            var fakeCiphertextDto1 = ciphertextDatastore.GetAll().First();

            // When
            ciphertextDatastore.Delete(fakeCiphertextDto1.Id);

            // Then
            ciphertextDatastore.GetAll(CiphertextStatus.Active)
                               .Should().NotContain(account => account.Id == fakeCiphertextDto1.Id);

            ciphertextDatastore.GetAll(CiphertextStatus.Deleted)
                               .Should().Contain(account => account.Id == fakeCiphertextDto1.Id);

            ciphertextDatastore.GetAllIds()
                               .Should().HaveCount(CiphertextFakeDataGenerator.PopulatedAccounts.Count(),
                               "Because GetAllIds returns also deleted items"); 
                                

            ciphertextDatastore.GetAll(CiphertextStatus.Active)
                               .Count()
                               .Should()
                               .BeLessThan(ciphertextDatastore.GetAll(CiphertextStatus.Any).Count());
        }
        [Test]
        public void WhenUpdatingAnAccount_ItShouldBeUpdated()
        {

            // Given
            var ciphertextDatastore = CreateReloadedPopulatedWithFakeData();
            var fakeCiphertextDto1 = ciphertextDatastore.GetAll().First();
            var originalCount = ciphertextDatastore.GetAll().Count();

            var updatedCiphertextDto = new CiphertextDto();

            // When
            updatedCiphertextDto.Id = fakeCiphertextDto1.Id;
            updatedCiphertextDto.Ciphertext = Convert.FromBase64String("rdLQpiBIHfXhZTdZMso8CDrj0KOY9Z+KFsgRjscuSoE=");
            updatedCiphertextDto.InitializationVector = Convert.FromBase64String("fAnP53b7vCuC2aabHQZvuA==");
            updatedCiphertextDto.Salt = Convert.FromBase64String("wu6rj5K00UFiakRwhzlbrHGD3AyK5bzW");

            ciphertextDatastore.Save(updatedCiphertextDto);

            // Then
            ciphertextDatastore.GetAll(CiphertextStatus.Active)
                               .Should().Contain(account => account.Id == updatedCiphertextDto.Id);

            ciphertextDatastore.GetAllIds()
                               .Should().HaveCount(originalCount);

            VerifyThatStoredDataMatches(ciphertextDatastore, updatedCiphertextDto);

        }



    }

}
