using System;
using PassFruit.AccountImplementations;
using PassFruit.Contracts;

namespace PassFruit.Tests.FakeData {

    public class FakeDataGenerator {

        public void GenerateFakeData(IRepository repository) {

            repository.AccountTags.Add(new AccountTag(Guid.NewGuid(), repository) {
                Name = @"Tag 1",
                Description = @"Description informations for Tag 1"
            });
            repository.AccountTags.Add(new AccountTag(Guid.NewGuid(), repository) {
                Name = @"Tag 2",
                Description = @"Description informations for Tag 2"
            });

            repository.Accounts.Add(
                new FacebookAccount(repository) {
                    Notes = @"Example test note 1",
                    Email = @"testFacebook@example.com"
                });
            
            repository.Accounts.Add(
                new TwitterAccount(repository) {
                    Notes = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet",
                    Email = @"testTwitter@example.com",
                    UserName = @"TwitterUser"
                });
            
            repository.Accounts.Add(
                new GoogleAccount(repository) {
                    Email = "user.name.example@gmail.com",
                    Notes = ""
                });
            
            repository.Accounts.Add(
                new GoogleAccount(repository) {
                    Email = "example2@gmail.com",
                    Notes = ""
                });

            repository.Accounts[0].SetPassword("Password1");
            repository.Accounts[1].SetPassword("Password2!£!$!$%!$&!£&!");
            repository.Accounts[2].SetPassword("Password3");
            repository.Accounts[3].SetPassword("Password4");

            repository.Accounts[0].AddTag("Tag 1");
            repository.Accounts[1].AddTag("Tag 1");
            repository.Accounts[0].AddTag("Tag 2");
            repository.Accounts[2].AddTag("Tag 2");

            foreach (var account in repository.Accounts) {
                account.SetClean();
            }

        }

    }
}
