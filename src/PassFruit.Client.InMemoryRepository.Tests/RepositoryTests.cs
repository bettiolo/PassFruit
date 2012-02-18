using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.AccountImplementations;
using PassFruit.Contracts;
using PassFruit.Tests.FakeData;

namespace PassFruit.Client.InMemoryRepository.Tests {

    [TestFixture]
    public class RepositoryTests {

        private InMemoryRepository GetRepositoryWithFakeData() {
            var repository = new InMemoryRepository();
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(repository);
            return repository;
        }

        [Test]
        public void When_An_Account_Is_Loaded_The_Correct_One_Should_Be_Returned() {
            
            // Given
            var repository = GetRepositoryWithFakeData();
            var testFacebookEmail = "testFacebook@example.com";
            var testTwitterAccount = "TwitterUser";
            var testTwitterEmail = "testTwitter@example.com";
            var testGoogleEmail = "user.name.example@gmail.com";
            
            // When
            var facebookAccount = repository.Accounts.GetByEmail(testFacebookEmail).FirstOrDefault();
            var twitterAccountByUserName = repository.Accounts.GetByUserName(testTwitterAccount).FirstOrDefault();
            var twitterAccountByEmail = repository.Accounts.GetByEmail(testTwitterEmail).FirstOrDefault();
            var gmailAccount = repository.Accounts.GetByEmail(testGoogleEmail).FirstOrDefault();

            // Then
            facebookAccount.Should().NotBeNull();
            facebookAccount.Email.Should().Be(testFacebookEmail);
            facebookAccount.Should().BeOfType<FacebookAccount>();

            twitterAccountByUserName.Should().NotBeNull();
            twitterAccountByUserName.UserName.Should().Be(testTwitterAccount);
            twitterAccountByUserName.Should().BeOfType<TwitterAccount>();

            twitterAccountByEmail.Should().NotBeNull();
            twitterAccountByEmail.Email.Should().Be(testTwitterEmail);
            twitterAccountByEmail.Should().BeOfType<TwitterAccount>();

            gmailAccount.Should().NotBeNull();
            gmailAccount.Email.Should().Be(testGoogleEmail);
            gmailAccount.Should().BeOfType<GoogleAccount>();

        }

        [Test]
        public void When_An_Account_Is_Added_Then_The_Number_Of_Accounts_Should_Increase() {
            
            // Given
            var repository = GetRepositoryWithFakeData();
            var originalAccountCount = repository.Accounts.Count();
            var facebookAccount = new FacebookAccount(repository);
            facebookAccount.Email = "testFacebookTemp@tin.it";

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
            accountWithUserName.UserName = "edItedUsEr";
            accountWithEmail.Email = editedEmail;
            accountWithUserName.Save();
            var retrievedAccount = (UserNameAndEmailAccountBase)repository.Accounts.GetByUserName(editedUser).First();

            // Then
            retrievedAccount.Id.Should().Be(originalId);
            retrievedAccount.UserName.Should().Be(editedUser);
            retrievedAccount.Email.Should().Be(editedEmail);


        }

    }

}
