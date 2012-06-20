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

        [Test]
        public void When_An_Account_Is_Loaded_The_Correct_One_Should_Be_Returned() {

            // Given
            var repository = GetRepositoryWithFakeData();
            var testFacebookEmail = FakeDataGenerator.FacebookEmail;
            var testTwitterAccount = FakeDataGenerator.TwitterUserName;
            var testTwitterEmail = FakeDataGenerator.TwitterEmail;
            var testGoogleEmail = FakeDataGenerator.Google1Email;
            IRepository reloadedRepository = null;

            // When
            var facebookAccount = repository.Accounts.GetByEmail(testFacebookEmail).FirstOrDefault();
            var twitterAccountByUserName = repository.Accounts.GetByUserName(testTwitterAccount).FirstOrDefault();
            var twitterAccountByEmail = repository.Accounts.GetByEmail(testTwitterEmail).FirstOrDefault();
            var gmailAccount = repository.Accounts.GetByEmail(testGoogleEmail).FirstOrDefault();
            Action actReloadRepository = () => reloadedRepository = GetReloadedRepository(); 

            // Then
            facebookAccount.Should().NotBeNull();
            facebookAccount.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testFacebookEmail);
            facebookAccount.Provider.Key.Should().Be("facebook");

            twitterAccountByUserName.Should().NotBeNull();
            twitterAccountByUserName.GetDefaultField(FieldTypeKey.UserName).Value.Should().Be(testTwitterAccount);
            twitterAccountByUserName.Provider.Key.Should().Be("twitter");

            twitterAccountByEmail.Should().NotBeNull();
            twitterAccountByEmail.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testTwitterEmail);
            twitterAccountByEmail.Provider.Key.Should().Be("twitter");

            gmailAccount.Should().NotBeNull();
            gmailAccount.GetDefaultField(FieldTypeKey.Email).Value.Should().Be(testGoogleEmail);
            gmailAccount.Provider.Key.Should().Be("google");

            actReloadRepository();
            reloadedRepository.Should().NotBeNull();

        }

        [Test]
        public void When_An_Account_Is_Added_Then_The_Number_Of_Accounts_Should_Increase() {

            // Given
            var repository = GetRepositoryWithFakeData();
            var originalAccountCount = repository.Accounts.Count();
            var facebookAccount = repository.Accounts.Create("generic");
            facebookAccount.SetField(FieldTypeKey.Email, "testFacebookTemp@tin.it");

            // When
            repository.Accounts.Add(facebookAccount);
            var repositorySaveAll = new Action(repository.SaveAll);

            // Then
            facebookAccount.IsDirty.Should().BeTrue();
            repositorySaveAll();
            facebookAccount.IsDirty.Should().BeFalse();
            repository.Accounts.Count.Should().Be(originalAccountCount + 1);

        }

        [Test]
        public void When_An_Account_Is_Deleted_Then_The_Number_Of_Accounts_Should_Decrease() {

            // Given
            var repository = GetRepositoryWithFakeData();
            var originalAccountCount = repository.Accounts.Count();
            var facebookAccount = repository.Accounts.GetByEmail("testFacebook@example.com").First();

            // When
            repository.Accounts.Remove(facebookAccount);

            // Then
            repository.Accounts.Count(a => a.GetType() != typeof(DeletedAccount))
                .Should().Be(originalAccountCount - 1);
            repository.Accounts.Should().Contain(a => a is DeletedAccount);

        }

        [Test]
        public void When_An_Account_Is_Edited_Then_The_Correct_Data_Should_Be_Retrieved() {

            // Given
            var editedUser = "edItedUsEr";
            var editedEmail = "editedEmail@twitter.example";
            var repository = GetRepositoryWithFakeData();
            var accountWithUserName = repository.Accounts.GetByUserName("tWiTTeRUsEr").First();
            var accountWithEmail = repository.Accounts.GetByEmail("testTwitter@example.com").First();
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
            var account = repository.Accounts.GetByUserName("tWiTTeRUsEr").First();
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
            var account = repository.Accounts.GetByUserName("tWiTTeRUsEr").First();
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
