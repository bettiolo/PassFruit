﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace PassFruit.Datastore.Tests {

    [TestFixture]
    public abstract class DatastoreTestsBase {

        protected abstract IDatastore CreateEmptyDatastore();

        protected abstract IDatastore CreateDatastoreWithFakeData();

        private void TestWithEmptyDatastore(Action<IDatastore> test) {
            test(CreateEmptyDatastore());
        }

        private void TestWithPrepopulatedDatastore(Action<IDatastore> test) {
            test(CreateDatastoreWithFakeData());
        }

        //private void TestWithBothDatastores(Action<IDatastore> test) {
        //    test(GetDatastoreWithFakeData());
        //    test(GetDatastore());
        //}

        private void TestAccountAndPassword(IDatastore dataStore, AccountDto accountDto) {
            dataStore.GetAllAccountIds().Should().Contain(id => id.Equals(accountDto.Id));
            dataStore.GetAccountDtos().Should().Contain(dataStoreAccountDto => accountDto.Id == dataStoreAccountDto.Id);
            dataStore.GetAccountDto(accountDto.Id).Equals(accountDto).Should().BeTrue();
            dataStore.GetAccountDto(accountDto.Id).IsDeleted.Should().Be(accountDto.IsDeleted);
            dataStore.GetAccountDto(accountDto.Id).Id.Should().Be(accountDto.Id);
            dataStore.GetAccountDto(accountDto.Id).LastChangedUtc.Should().Be(accountDto.LastChangedUtc);
            dataStore.GetAccountDto(accountDto.Id).Notes.Should().Be(accountDto.Notes);
            dataStore.GetAccountDto(accountDto.Id).ProviderKey.Should().Be(accountDto.ProviderKey);
            dataStore.GetAccountDto(accountDto.Id).Tags.Should().Equal(accountDto.Tags);
            dataStore.GetAccountDto(accountDto.Id).Fields.Should().Equal(accountDto.Fields);
            //dataStore.GetPasswordDtos(accountDto.Id).Should().HaveCount(passwordDtos.Count());
            //dataStore.GetPasswordDtos(accountDto.Id).First().Equals(passwordDtos.First()).Should().BeTrue();
            //dataStore.GetPasswordDtos(accountDto.Id).Should().Equal(passwordDtos);
        }

        [Test]
        public void WhenAddingASingleAccount_ItShouldBeAdded() {

            // Given
            var facebookAccount = FakeData.FakeDataGenerator.GetFacebookAccount();
            var facebookPassword = FakeData.FakeDataGenerator.GetFacebookPassword();
            facebookAccount.Fields.Add(facebookPassword);
            
            IDatastore originalDatastore = null;

            // When
            TestWithEmptyDatastore(dataStore => {
                dataStore.SaveAccountDto(facebookAccount);
                originalDatastore = dataStore;
            });
            
            // Then
            originalDatastore.GetAllAccountIds().Should().HaveCount(1);
            originalDatastore.GetAccountDtos().Should().HaveCount(1);
            TestAccountAndPassword(originalDatastore, facebookAccount);
        }

        [Test]
        public void WhenAddingMultipeAccounts_TheyShouldBeAdded() {

            // Given
            var facebookAccount = FakeData.FakeDataGenerator.GetFacebookAccount();
            var facebookPassword = FakeData.FakeDataGenerator.GetFacebookPassword();
            facebookAccount.Fields.Add(facebookPassword);

            var twitterAccount = FakeData.FakeDataGenerator.GetTwitterAccount();
            var twitterPassword = FakeData.FakeDataGenerator.GetTwitterPassword();
            facebookAccount.Fields.Add(twitterPassword);

            IDatastore originalDatastore = null;

            // When
            TestWithEmptyDatastore(dataStore =>
            {
                dataStore.SaveAccountDto(facebookAccount);
                dataStore.SaveAccountDto(twitterAccount);
                originalDatastore = dataStore;
            });

            // Then
            originalDatastore.GetAccountDtos().Should().HaveCount(2);
            TestAccountAndPassword(originalDatastore, facebookAccount);
            TestAccountAndPassword(originalDatastore, twitterAccount);
   
        }

        [Test]
        public void WhenDeletingAnAccount_ItShouldBeDeleted() {

            // Given
            IDatastore originalDatastore = null;
            AccountDto twitterAccount = null;

            // When
            TestWithPrepopulatedDatastore(dataStore => {
                originalDatastore = dataStore;
                twitterAccount =
                    dataStore.GetAccountDtos().First(account => 
                        account.ProviderKey == FakeData.FakeDataGenerator.TwitterProviderKey);
                dataStore.DeleteAccountDto(twitterAccount.Id);
            });

            // Then
            originalDatastore.GetAccountDtos().Should().NotContain(account => account.Id == twitterAccount.Id);
            originalDatastore.GetAccountDtos().Should().NotContain(account => account.ProviderKey == FakeData.FakeDataGenerator.TwitterProviderKey);
        }

        [Test]
        public void WhenDeletingAPassword_ItShouldBeDeleted() {

            // Given
            IDatastore originalDatastore = null;
            AccountDto twitterAccount = null;
            FieldDto originalPasswordDto = null;
            FieldDto changedPasswordDto = FakeData.FakeDataGenerator.GetFacebookPassword();

            // When
            TestWithPrepopulatedDatastore(dataStore =>
            {
                originalDatastore = dataStore;
                twitterAccount =
                    dataStore.GetAccountDtos().First(account =>
                        account.ProviderKey == FakeData.FakeDataGenerator.TwitterProviderKey);
                originalPasswordDto = twitterAccount.Fields.Single(field => field.FieldTypeKey == "pin");
                twitterAccount.Fields.Remove(originalPasswordDto);
                twitterAccount.Fields.Add(changedPasswordDto);
                originalDatastore.SaveAccountDto(twitterAccount);
            });

            // Then
            originalDatastore.GetAccountDto(twitterAccount.Id).Fields.Should().NotContain(originalPasswordDto);
            originalDatastore.GetAccountDto(twitterAccount.Id).Fields.Should().Contain(changedPasswordDto);
        }

        /*
        [Test]
        public void When_An_Account_Is_Loaded_The_Correct_One_Should_Be_Returned() {

            // Given
            IAccountDto facebookAccount = null;
            IAccountDto twitterAccountByUserName = null;
            IAccountDto twitterAccountByEmail = null;
            IAccountDto gmailAccount = null;
            const string testFacebookEmail = FakeDataGenerator.FacebookEmail;
            const string testTwitterAccount = FakeDataGenerator.TwitterUserName;
            const string testTwitterEmail = FakeDataGenerator.TwitterEmail;
            const string testGoogleEmail = FakeDataGenerator.Google1Email;

            // When
            Action<IDatastore> actLoadAccounts = repository => {
                facebookAccount = repository.Accounts.GetByEmail(testFacebookEmail).FirstOrDefault();
                twitterAccountByUserName = repository.Accounts.GetByUserName(testTwitterAccount).FirstOrDefault();
                twitterAccountByEmail = repository.Accounts.GetByEmail(testTwitterEmail).FirstOrDefault();
                gmailAccount = repository.Accounts.GetByEmail(testGoogleEmail).FirstOrDefault();
            };

            // Then
            TestWithBothDatastores(repository => {

                actLoadAccounts(repository);

                facebookAccount.Should().NotBeNull();
                facebookAccount.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testFacebookEmail);
                facebookAccount.Provider.Should().NotBeNull();
                facebookAccount.Provider.Key.Should().Be(FakeDataGenerator.FacebookProviderKey);

                twitterAccountByUserName.Should().NotBeNull();
                twitterAccountByUserName.GetDefaultField(FieldTypeKey.UserName).Value.Should().Be(testTwitterAccount);
                twitterAccountByUserName.Provider.Should().NotBeNull();
                twitterAccountByUserName.Provider.Key.Should().Be(FakeDataGenerator.TwitterProviderKey);

                twitterAccountByEmail.Should().NotBeNull();
                twitterAccountByEmail.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testTwitterEmail);
                twitterAccountByEmail.Provider.Should().NotBeNull();
                twitterAccountByEmail.Provider.Key.Should().Be(FakeDataGenerator.TwitterProviderKey);

                gmailAccount.Should().NotBeNull();
                gmailAccount.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testGoogleEmail);
                gmailAccount.Provider.Should().NotBeNull();
                gmailAccount.Provider.Key.Should().Be(FakeDataGenerator.GoogleProviderKey);

            });

        }

        [Test]
        public void When_An_Account_Is_Added_Then_The_Number_Of_Accounts_Should_Increase() {

            // Given
            const string newEmail = "testFacebookTemp@tin.it";
            var originalAccountCount = 0;
            IAccount facebookAccount = null;

            // When
            Action<IDatastore> actLoadAccount = repository => {
                originalAccountCount = repository.Accounts.Count();
                facebookAccount = repository.Accounts.Create(FakeDataGenerator.GenericProviderKey);
            };
            Action<IDatastore> actEditAccount = repository =>
                facebookAccount.SetField(FieldTypeKey.Email, newEmail);
            Action<IDatastore> actAddAccount = repository =>
                repository.Accounts.Add(facebookAccount);
            Action<IDatastore> actRepositorySaveAll = repository =>
                repository.SaveAll();

            // Then
            TestWithBothDatastores(repository => {
                actLoadAccount(repository);
                actEditAccount(repository);
                facebookAccount.IsDirty.Should().BeTrue();
                repository.Accounts.Count.Should().Be(originalAccountCount);
                actAddAccount(repository);
                actRepositorySaveAll(repository);
                facebookAccount.IsDirty.Should().BeFalse();
                repository.Accounts.Count.Should().Be(originalAccountCount + 1);
            });
        }

        [Test]
        public void When_An_Account_Is_Deleted_Then_The_Number_Of_Accounts_Should_Decrease() {

            // Given
            var originalAccountCount = 0;
            IAccount facebookAccount = null;
            IAccount deletedAccount = null;
            var facebookAccountId = Guid.Empty;

            // When
            Action<IDatastore> actLoadAccount = repository => {
                originalAccountCount = repository.Accounts.Count();
                facebookAccount = repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).First();
                facebookAccountId = facebookAccount.Id;
            };
            Action<IDatastore> actRemoveAccount = repository =>
                repository.Accounts.Remove(facebookAccount);
            Action<IDatastore> actLoadDeletedAccount = repository =>
                deletedAccount = repository.Accounts.First(a => a is DeletedAccount);

            // Then
            TestWithPrepopulatedReloadedDatastore(repository => {
                actLoadAccount(repository);
                for (int i = 0; i < 3; i++) {
                    if (i < 2) {
                        actRemoveAccount(repository);
                    } else if (i == 2) {
                        actLoadDeletedAccount(repository);
                        repository.Accounts.Remove(deletedAccount);
                    }
                    repository.Accounts.Count().Should().Be(originalAccountCount);
                    repository.Accounts.Should().Contain(a => a is DeletedAccount);
                    repository.Accounts.Count(a => a is DeletedAccount).Should().Be(1);
                    repository.Accounts.First(a => a is DeletedAccount).IsDirty.Should().BeTrue();
                    repository.IsDirty().Should().BeTrue();
                }
                repository.GetPasswordDto(facebookAccount.Id).Should().BeBlank();
                repository.IsDirty().Should().BeTrue();
                repository.SaveAll();
                facebookAccount.GetPassword().Should().BeBlank();
                repository.GetPasswordDto(facebookAccount.Id).Should().BeBlank();
                repository.IsDirty().Should().BeFalse();
                repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).Should().BeEmpty();
                repository.Accounts.Count().Should().Be(originalAccountCount - 1);
                repository.Accounts.Should().NotContain(a => a is DeletedAccount);
            });


            TestWithReloadedDatastore(repository => {
                repository.IsDirty().Should().BeFalse();
                repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).Should().BeEmpty();
                repository.Accounts.Count().Should().Be(originalAccountCount - 1);
                repository.GetPasswordDto(facebookAccountId).Should().BeBlank();
                var deletedAccounts = repository.GetDeletedAccounts();
                deletedAccounts.Any().Should().BeTrue();
                deletedAccounts.Should().Contain(account => account.Id == facebookAccountId);
            });

        }

        [Test]
        public void When_An_Account_Is_Edited_Then_The_Correct_Data_Should_Be_Retrieved() {

            // Given
            const string editedUser = "edItedUsEr";
            const string editedEmail = "editedEmail@twitter.example";
            const string editedNote = "edited note text bla bla bla! ¬!\"£$%^&*()_+=-`[]#'/.,<>?|\\";
            IAccount accountWithUserName = null;
            var originalId = Guid.Empty;

            // When
            Action<IDatastore> actLoadAndEditAccount = repository => {
                accountWithUserName = repository.Accounts.GetByUserName(FakeDataGenerator.TwitterUserName).First();
                originalId = accountWithUserName.Id;
                accountWithUserName.SetField(FieldTypeKey.UserName, editedUser);
                accountWithUserName.SetField(FieldTypeKey.Email, editedEmail);
                accountWithUserName.Notes = editedNote;
                accountWithUserName.Save();
            };

            // Then
            TestWithBothDatastores(repository => {
                if (accountWithUserName == null) {
                    actLoadAndEditAccount(repository);
                }
                var retrievedAccount = repository.Accounts.GetByUserName(editedUser).First();
                retrievedAccount.Id.Should().Be(originalId);
                retrievedAccount.GetDefaultField(FieldTypeKey.UserName).Value.Should().Be(editedUser);
                retrievedAccount.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(editedEmail);
                retrievedAccount.Notes.Should().Be(editedNote);
            });

        }

        [Test]
        public void When_A_Tag_Is_Added_Then_The_Number_Of_Tags_Should_Increase() {

            // Given
            IAccount twitterAccount = null;
            var originalTotalTagCount = -1;
            var originalAccountTagCount = -1;
            const string testTagA = "test-tag-A";
            const string testTagB = "test-tag-B";
            const string testNonValidTag = "unvalid tag";

            // When
            Action<IAccount> actAddTag = account => account.Tags.Add(testTagA);
            Action<IAccount> actSave = account => account.Save();
            Action<IDatastore> actLoadAccount = repository =>
                twitterAccount = repository.Accounts.GetByUserName(FakeDataGenerator.TwitterUserName).First();
            
            // Then
            TestWithUnsavedFakeData(repository => {
                actLoadAccount(repository);
                originalTotalTagCount = repository.GetAllTags().Count();
                originalAccountTagCount = twitterAccount.Tags.Count();
                twitterAccount.IsDirty.Should().BeFalse();
                actAddTag(twitterAccount);
                twitterAccount.IsDirty.Should().BeTrue();
                repository.GetAllTags().Count().Should().Be(originalTotalTagCount + 1);
                repository.GetAllTags().Count().Should().BeGreaterOrEqualTo(twitterAccount.Tags.Count());
                actSave(twitterAccount);
            });
            TestWithReloadedDatastore(repository => {
                actLoadAccount(repository);
                twitterAccount.IsDirty.Should().BeFalse();
                twitterAccount.Tags.Count().Should().Be(originalAccountTagCount + 1);
                repository.GetAllTags().Count().Should().Be(originalTotalTagCount + 1);
                repository.GetAllTags().Any(tag => tag.Key.Equals(testTagA, StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
                repository.GetAllTags().Any(tag => tag.Key.Equals(testTagB, StringComparison.OrdinalIgnoreCase)).Should().BeFalse();
                repository.GetAllTags().Count().Should().BeGreaterOrEqualTo(twitterAccount.Tags.Count());
                var accounts = repository.GetAllTags().First(tag => tag.Key.Equals(testTagA, StringComparison.OrdinalIgnoreCase)).Accounts;
                accounts.Should().Contain(account => account.Equals(twitterAccount));
            });
        }

        [Test]
        public void When_A_Tag_Is_Deleted_Then_The_Number_Of_Tags_Should_Decrease() {
            // Given
            IAccount googleAccount = null;
            var originalTotalTagCount = -1;
            var originalAccountTagCount = -1;

            // When
            Action<IDatastore> actLoadAccount = repository => 
                googleAccount = repository.Accounts.GetByEmail(FakeDataGenerator.Google1Email).First();
            Action actDeleteTag = () => {
                googleAccount.Tags.Remove(FakeDataGenerator.Tag1);
                googleAccount.Tags.Remove(FakeDataGenerator.Tag2);
                googleAccount.Tags.Remove(FakeDataGenerator.Tag3);
            };
            Action actSave = () => googleAccount.Save();

            // Then
            TestWithUnsavedFakeData(repository => {
                actLoadAccount(repository);
                originalTotalTagCount = repository.GetAllTags().Count();
                originalAccountTagCount = googleAccount.Tags.Count();
                googleAccount.IsDirty.Should().BeFalse();
                actDeleteTag();
                googleAccount.IsDirty.Should().BeTrue();
                googleAccount.Tags.Count().Should().Be(0);
                repository.GetAllTags().Count().Should().BeGreaterOrEqualTo(googleAccount.Tags.Count());
                actSave();
            });
            TestWithReloadedDatastore(repository => {
                actLoadAccount(repository);
                googleAccount.IsDirty.Should().BeFalse();
                googleAccount.Tags.Count().Should().Be(0);
                repository.GetAllTags().Count().Should().BeGreaterOrEqualTo(0);
                repository.GetAllTags().Any(tag => tag.Key.Equals(FakeDataGenerator.Tag1, StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
            });
        }

        [Test]
        public void When_A_Password_Is_Changed_Then_The_Account_Should_Not_Be_Dirty_And_The_Password_Shold_Be_Saved() {

            // Given
            var repository = GetDatastoreWithFakeData();
            var account = repository.Accounts.GetByUserName(FakeDataGenerator.TwitterUserName).First();
            var originalDefaultPassword = account.GetPassword();
            var originalCustomPassword = account.GetPassword("custom");
            var newDefaultPassword = "new password 123131 \"\"\"\"$£&!£&!\'\'\'\'|!£!><><>><";
            var newCustomPassword = "new Pasw0rd";
            var newEmail = "test@example.org";

            // When
            var actChangeDefaultPassword = new Action(() => account.SetPassword(newDefaultPassword));
            var actChangeCustomPassword = new Action(() => account.SetPassword(newCustomPassword, "CuStoM"));
            var actChangeEmail = new Action(() => account.SetField(FieldTypeKey.Email, newEmail));
            var actSave = new Action(account.Save);

            // Then
            account.IsDirty.Should().BeFalse();
            account.GetPassword().Should().Be(originalDefaultPassword);
            account.GetPassword("CUSToM").Should().Be(originalCustomPassword);
            actChangeDefaultPassword();
            actChangeCustomPassword();
            account.IsDirty.Should().BeFalse(); // If a password is changed the dirty flag should not be set.
            actChangeEmail();
            account.IsDirty.Should().BeTrue();
            actSave();
            account.IsDirty.Should().BeFalse();
            account.GetPassword().Should().Be(newDefaultPassword);
            account.GetPassword("CUStom").Should().Be(newCustomPassword);
            account.GetFieldsByKey(FieldTypeKey.Email).First().Value.Should().Be(newEmail);

        }

        [Test]
        public void When_Saving_the_same_data_the_data_should_not_be_saved() {
            // check lastchangedutc
        }

         */

        }

}
