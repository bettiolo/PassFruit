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
                                                             CipheredAccountDto cipheredAccountDto)
        {
            var accountId = cipheredAccountDto.Id;
            ciphertextDatastore.GetAllIds()
                               .Should().Contain(id => id.Equals(accountId));

            ciphertextDatastore.GetAll()
                               .Should().Contain(datastoreCipheredAccountDto => accountId == datastoreCipheredAccountDto.Id);

            ciphertextDatastore.Get(accountId).Equals(cipheredAccountDto)
                               .Should().BeTrue();

            ciphertextDatastore.Get(accountId).Deleted
                               .Should().Be(cipheredAccountDto.Deleted);

            ciphertextDatastore.Get(accountId).Id
                               .Should().Be(cipheredAccountDto.Id);

            ciphertextDatastore.Get(accountId).Ciphertext
                               .Should().Equal(cipheredAccountDto.Ciphertext);

            ciphertextDatastore.Get(accountId).InitializationVector
                               .Should().Equal(cipheredAccountDto.InitializationVector);

            ciphertextDatastore.Get(accountId).Salt
                               .Should().Equal(cipheredAccountDto.Salt);
        }

        private void WhenAddingASingleAccountToADataStore_ItShouldBeAdded(CiphertextDatastoreBase ciphertextDatastore, int expectedCount)
        {

            // Given
            var fakeCipheredAccount = CipheredAccountFakeDataGenerator.GetFakeCipheredAccountDtoNotYetAdded();

            // When
            ciphertextDatastore.Save(fakeCipheredAccount);

            // Then
            ciphertextDatastore.GetAllIds()
                               .Should().HaveCount(expectedCount);

            ciphertextDatastore.GetAll(CipheredAccountStatus.Any)
                               .Should().HaveCount(expectedCount);

            ciphertextDatastore.GetAll(CipheredAccountStatus.Active)
                               .Should().HaveCount(expectedCount);

            ciphertextDatastore.GetAll(CipheredAccountStatus.Deleted)
                               .Should().HaveCount(0);

            VerifyThatStoredDataMatches(ciphertextDatastore, fakeCipheredAccount);
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
                CipheredAccountFakeDataGenerator.PopulatedAccounts.Length + 1);
        }

        [Test]
        public void WhenAddingASingleAccountToAReloadedPopulatedDataStore_ItShouldBeAdded()
        {
            WhenAddingASingleAccountToADataStore_ItShouldBeAdded(
                CreateReloadedPopulatedWithFakeData(),
                CipheredAccountFakeDataGenerator.PopulatedAccounts.Length + 1);
        }

        [Test]
        public void WhenAddingMultipeAccounts_TheyShouldBeAdded()
        {

            // Given
            var fakeCipheredAccountDto1 = CipheredAccountFakeDataGenerator.GetFakeCipheredAccountDto1();
            var fakeCipheredAccountDto2 = CipheredAccountFakeDataGenerator.GetFakeCipheredAccountDto2();
            var fakeCipheredAccountDtoNotYetAdded =
                CipheredAccountFakeDataGenerator.GetFakeCipheredAccountDtoNotYetAdded();

            var ciphertextDatastore = CreateEmpty();

            // When
            ciphertextDatastore.Save(fakeCipheredAccountDto1);
            ciphertextDatastore.Save(fakeCipheredAccountDto2);

            // Then
            fakeCipheredAccountDtoNotYetAdded.Id
                   .Should().BeEmpty();

            ciphertextDatastore.GetAll(CipheredAccountStatus.Any)
                               .Should().HaveCount(2);

            ciphertextDatastore.GetAll(CipheredAccountStatus.Deleted)
                               .Should().HaveCount(0);

            ciphertextDatastore.GetAll(CipheredAccountStatus.Active)
                               .Should().HaveCount(2);

            VerifyThatStoredDataMatches(ciphertextDatastore, fakeCipheredAccountDto1);
            VerifyThatStoredDataMatches(ciphertextDatastore, fakeCipheredAccountDto2);
        }

        [Test]
        public void WhenDeletingAnAccountThatHasBeenAddedToAnEmptyDatastore_ItShouldBeDeleted()
        {

            // Given
            var fakeCipheredAccountDto1 = CipheredAccountFakeDataGenerator.GetFakeCipheredAccountDto1();

            var ciphertextDatastore = CreateEmpty();

            // When
            ciphertextDatastore.Save(fakeCipheredAccountDto1);
            ciphertextDatastore.Delete(fakeCipheredAccountDto1.Id);

            // Then
            ciphertextDatastore.GetAll(CipheredAccountStatus.Active)
                               .Should().NotContain(account => account.Id == fakeCipheredAccountDto1.Id);

            ciphertextDatastore.GetAll(CipheredAccountStatus.Deleted)
                               .Should().Contain(account => account.Id == fakeCipheredAccountDto1.Id);

            ciphertextDatastore.GetAllIds()
                               .Should().HaveCount(1); // Because AllIds return also deleted ones

            ciphertextDatastore.GetAll(CipheredAccountStatus.Active).Count()
                               .Should().BeLessThan(ciphertextDatastore.GetAll(CipheredAccountStatus.Any).Count());

            ciphertextDatastore.Get(fakeCipheredAccountDto1.Id).Id
                               .Should().NotBeEmpty();

            ciphertextDatastore.Get(fakeCipheredAccountDto1.Id).Ciphertext
                               .Should().BeEmpty();

            ciphertextDatastore.Get(fakeCipheredAccountDto1.Id).Deleted
                               .Should().BeTrue();

            ciphertextDatastore.Get(fakeCipheredAccountDto1.Id).InitializationVector
                               .Should().BeEmpty();

            ciphertextDatastore.Get(fakeCipheredAccountDto1.Id).Salt
                               .Should().BeEmpty();

            ciphertextDatastore.Get(fakeCipheredAccountDto1.Id).IsNew()
                               .Should().BeFalse();

        }

        [Test]
        public void WhenDeletingAnAccountThatHasBeenAddedToAPopulatedDatastore_ItShouldBeDeleted()
        {

            // Given
            var ciphertextDatastore = CreatePopulatedWithFakeData();
            var fakeCipheredAccountDto1 = ciphertextDatastore.GetAll().First();

            // When
            ciphertextDatastore.Delete(fakeCipheredAccountDto1.Id);

            // Then
            ciphertextDatastore.GetAll(CipheredAccountStatus.Active)
                               .Should().NotContain(account => account.Id == fakeCipheredAccountDto1.Id);

            ciphertextDatastore.GetAll(CipheredAccountStatus.Deleted)
                               .Should().Contain(account => account.Id == fakeCipheredAccountDto1.Id);

            ciphertextDatastore.GetAllIds()
                               .Should().HaveCount(CipheredAccountFakeDataGenerator.PopulatedAccounts.Count(),
                               "Because GetAllIds returns also deleted items"); 
                                

            ciphertextDatastore.GetAll(CipheredAccountStatus.Active)
                               .Count()
                               .Should()
                               .BeLessThan(ciphertextDatastore.GetAll(CipheredAccountStatus.Any).Count());
        }
        [Test]
        public void WhenUpdatingAnAccount_ItShouldBeUpdated()
        {

            // Given
            var ciphertextDatastore = CreateReloadedPopulatedWithFakeData();
            var fakeCipheredAccountDto1 = ciphertextDatastore.GetAll().First();
            var originalCount = ciphertextDatastore.GetAll().Count();

            var updatedCipheredAccountDto = new CipheredAccountDto();

            // When
            updatedCipheredAccountDto.Id = fakeCipheredAccountDto1.Id;
            updatedCipheredAccountDto.Ciphertext = Convert.FromBase64String("rdLQpiBIHfXhZTdZMso8CDrj0KOY9Z+KFsgRjscuSoE=");
            updatedCipheredAccountDto.InitializationVector = Convert.FromBase64String("fAnP53b7vCuC2aabHQZvuA==");
            updatedCipheredAccountDto.Salt = Convert.FromBase64String("wu6rj5K00UFiakRwhzlbrHGD3AyK5bzW");

            ciphertextDatastore.Save(updatedCipheredAccountDto);

            // Then
            ciphertextDatastore.GetAll(CipheredAccountStatus.Active)
                               .Should().Contain(account => account.Id == updatedCipheredAccountDto.Id);

            ciphertextDatastore.GetAllIds()
                               .Should().HaveCount(originalCount);

            VerifyThatStoredDataMatches(ciphertextDatastore, updatedCipheredAccountDto);

        }



    }

}
