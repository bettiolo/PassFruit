using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Contracts;
using PassFruit.Tests.FakeData;

namespace PassFruit.Tests {

    [TestFixture]
    public abstract class RepositoryTestsBase {

        protected abstract IRepository GetNewRepositoryWithFakeData();

        protected abstract IRepository GetReloadedRepository();

        private void TestWithUnsavedFakeData(Action<IRepository> test) {
            test(GetNewRepositoryWithFakeData());
        }

        private void TestWithReloadedRepository(Action<IRepository> test) {
            test(GetReloadedRepository());
        }

        private void TestWithPrepopulatedReloadedRepository(Action<IRepository> test) {
            GetNewRepositoryWithFakeData().SaveAll();
            test(GetReloadedRepository());
        }

        private void TestWithBothRepositories(Action<IRepository> test) {
            test(GetNewRepositoryWithFakeData());
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
            Action<IRepository> actLoadAccount = repository => {
                originalAccountCount = repository.Accounts.Count();
                facebookAccount = repository.Accounts.Create(FakeDataGenerator.GenericProviderKey);
            };
            Action<IRepository> actEditAccount = repository =>
                facebookAccount.SetField(FieldTypeKey.Email, newEmail);
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
            var facebookAccountId = Guid.Empty;

            // When
            Action<IRepository> actLoadAccount = repository => {
                originalAccountCount = repository.Accounts.Count();
                facebookAccount = repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).First();
                facebookAccountId = facebookAccount.Id;
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
                repository.IsDirty().Should().BeTrue();
                repository.SaveAll();
                facebookAccount.GetPassword().Should().BeBlank();
                repository.GetPassword(facebookAccount.Id).Should().BeBlank();
                repository.IsDirty().Should().BeFalse();
                repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).Should().BeEmpty();
                repository.Accounts.Count().Should().Be(originalAccountCount - 1);
                repository.Accounts.Should().NotContain(a => a is DeletedAccount);
            });


            TestWithReloadedRepository(repository => {
                repository.IsDirty().Should().BeFalse();
                repository.Accounts.GetByEmail(FakeDataGenerator.FacebookEmail).Should().BeEmpty();
                repository.Accounts.Count().Should().Be(originalAccountCount - 1);
                repository.GetPassword(facebookAccountId).Should().BeBlank();
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
            Action<IRepository> actLoadAndEditAccount = repository => {
                accountWithUserName = repository.Accounts.GetByUserName(FakeDataGenerator.TwitterUserName).First();
                originalId = accountWithUserName.Id;
                accountWithUserName.SetField(FieldTypeKey.UserName, editedUser);
                accountWithUserName.SetField(FieldTypeKey.Email, editedEmail);
                accountWithUserName.Notes = editedNote;
                accountWithUserName.Save();
            };

            // Then
            TestWithBothRepositories(repository => {
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
            Action<IRepository> actLoadAccount = repository =>
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
            TestWithReloadedRepository(repository => {
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
            Action<IRepository> actLoadAccount = repository => 
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
            TestWithReloadedRepository(repository => {
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
            var repository = GetNewRepositoryWithFakeData();
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
