using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Contracts;
using PassFruit.Tests.FakeData;

namespace PassFruit.Tests {

    [TestFixture]
    public abstract class RepositoryTests {

        protected abstract IRepository GetRepositoryWithFakeData();

        protected abstract IRepository GetReloadedRepository();

        private void TestWithLoadedRepository(Action<IRepository> test) {
            test(GetReloadedRepository());
        }

        private void TestWithPrepopulatedReloadedRepository(Action<IRepository> test) {
            GetRepositoryWithFakeData().SaveAll();
            test(GetReloadedRepository());
        }

        private void TestWithBothRepositories(Action<IRepository> test) {
            test(GetRepositoryWithFakeData());
            test(GetReloadedRepository());
        }

        [Test]
        public void When_An_Account_Is_Loaded_The_Correct_One_Should_Be_Returned() {

            // Given
            IAccount facebookAccount = null;
            IAccount twitterAccountByUserName = null;
            IAccount twitterAccountByEmail = null;
            IAccount gmailAccount = null;
            const string testFacebookEmail = FakeDataGenerator.FacebookEmail;
            const string testTwitterAccount = FakeDataGenerator.TwitterUserName;
            const string testTwitterEmail = FakeDataGenerator.TwitterEmail;
            const string testGoogleEmail = FakeDataGenerator.Google1Email;

            // When
            Action<IRepository> actLoadAccounts = repository => {
                facebookAccount = repository.Accounts.GetByEmail(testFacebookEmail).FirstOrDefault();
                twitterAccountByUserName = repository.Accounts.GetByUserName(testTwitterAccount).FirstOrDefault();
                twitterAccountByEmail = repository.Accounts.GetByEmail(testTwitterEmail).FirstOrDefault();
                gmailAccount = repository.Accounts.GetByEmail(testGoogleEmail).FirstOrDefault();
            };

            // Then
            TestWithBothRepositories(repository => {
                
                actLoadAccounts(repository);

                facebookAccount.Should().NotBeNull();
                facebookAccount.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testFacebookEmail);
                facebookAccount.Provider.Should().NotBeNull();
                facebookAccount.Provider.Key.Should().Be("facebook");

                twitterAccountByUserName.Should().NotBeNull();
                twitterAccountByUserName.GetDefaultField(FieldTypeKey.UserName).Value.Should().Be(testTwitterAccount);
                twitterAccountByUserName.Provider.Should().NotBeNull();
                twitterAccountByUserName.Provider.Key.Should().Be("twitter");

                twitterAccountByEmail.Should().NotBeNull();
                twitterAccountByEmail.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testTwitterEmail);
                twitterAccountByEmail.Provider.Should().NotBeNull();
                twitterAccountByEmail.Provider.Key.Should().Be("twitter");

                gmailAccount.Should().NotBeNull();
                gmailAccount.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testGoogleEmail);
                gmailAccount.Provider.Should().NotBeNull();
                gmailAccount.Provider.Key.Should().Be("google");

            });

        }

        [Test]
        public void When_An_Account_Is_Added_Then_The_Number_Of_Accounts_Should_Increase() {

            // Given
            var originalAccountCount = 0;
            IAccount facebookAccount = null;

            // When
            Action<IRepository> actLoadAccount = repository => {
                originalAccountCount = repository.Accounts.Count();
                facebookAccount = repository.Accounts.Create("generic");
            };
            Action<IRepository> actEditAccount = repository => 
                facebookAccount.SetField(FieldTypeKey.Email, "testFacebookTemp@tin.it");
            Action<IRepository> actAddAccount = repository => 
                repository.Accounts.Add(facebookAccount);
            Action<IRepository> actRepositorySaveAll = repository => 
                repository.SaveAll();

            // Then
            TestWithBothRepositories(repository => {
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

            // When
            Action<IRepository> actLoadAccount = repository => {
                originalAccountCount = repository.Accounts.Count();
                facebookAccount = repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).First();
            };
            Action<IRepository> actRemoveAccount = repository =>
                repository.Accounts.Remove(facebookAccount);
            Action<IRepository> actLoadDeletedAccount = repository =>
                deletedAccount = repository.Accounts.First(a => a is DeletedAccount);

            // Then
            TestWithPrepopulatedReloadedRepository(repository => {
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
                repository.GetPassword(facebookAccount.Id).Should().BeBlank();
                repository.IsDirty().Should().BeFalse();
                repository.SaveAll();
                facebookAccount.GetPassword().Should().BeBlank();
                repository.GetPassword(facebookAccount.Id).Should().BeBlank();
                repository.IsDirty().Should().BeFalse();
                repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).Should().BeEmpty();
                repository.Accounts.Count().Should().Be(originalAccountCount - 1);
                repository.Accounts.Should().NotContain(a => a is DeletedAccount);
            });

            TestWithLoadedRepository(repository => {
                repository.IsDirty().Should().BeFalse();
                repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).Should().BeEmpty();
                repository.Accounts.Count().Should().Be(originalAccountCount - 1);
                repository.GetPassword(facebookAccount.Id).Should().BeBlank();
            });

        }

        [Test]
        public void When_An_Account_Is_Edited_Then_The_Correct_Data_Should_Be_Retrieved() {

            // Given
            var editedUser = "edItedUsEr";
            var editedEmail = "editedEmail@twitter.example";
            var repository = GetRepositoryWithFakeData();
            var accountWithUserName = repository.Accounts.GetByUserName(FakeDataGenerator.TwitterUserName).First();
            var accountWithEmail = repository.Accounts.GetByEmail(FakeDataGenerator.TwitterEmail).First();
            var originalId = accountWithUserName.Id;

            // When
            accountWithUserName.SetField(FieldTypeKey.UserName, editedUser);
            accountWithUserName.SetField(FieldTypeKey.Email, editedEmail);
            accountWithUserName.Save();
            var retrievedAccount = repository.Accounts.GetByUserName(editedUser).First();

            // Then
            retrievedAccount.Id.Should().Be(originalId);
            retrievedAccount.GetDefaultField(FieldTypeKey.UserName).Value.Should().Be(editedUser);
            retrievedAccount.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(editedEmail);

        }

        [Test]
        public void When_A_Tag_Is_Added_Then_The_Number_Of_Tags_Should_Increase() {

            // Given
            var repository = GetRepositoryWithFakeData();
            var account = repository.Accounts.GetByUserName(FakeDataGenerator.TwitterUserName).First();
            var originalTotalTagCount = repository.Tags.Count();
            var originalAccountTagCount = account.Tags.Count();

            // When
            var actAddTag = new Action(() => account.AddTag("test tag A"));
            var actSave = new Action(account.Save);

            // Then
            account.IsDirty.Should().BeFalse();
            actAddTag();
            account.IsDirty.Should().BeTrue();
            repository.Tags.Count().Should().Be(originalTotalTagCount);
            actSave();
            account.IsDirty.Should().BeFalse();
            account.Tags.Count().Should().Be(originalAccountTagCount + 1);
            repository.Tags.Count().Should().Be(originalTotalTagCount + 1);
            repository.Tags.Contains("test tag b").Should().BeFalse();
            repository.Tags.Contains("test tag a").Should().BeTrue();

        }

        [Test]
        public void When_A_Tag_Is_Deleted_Then_The_Number_Of_Tags_Should_Decrease() {
            throw new NotImplementedException();
        }


        [Test]
        public void When_A_Tag_Is_Changed_Then_The_Change_Should_Be_Persisted() {
            throw new NotImplementedException();
        }

        [Test]
        public void When_A_Password_Is_Changed_Then_The_Account_Should_Not_Be_Dirty_And_The_Password_Shold_Be_Saved() {

            // Given
            var repository = GetRepositoryWithFakeData();
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

    }

}
