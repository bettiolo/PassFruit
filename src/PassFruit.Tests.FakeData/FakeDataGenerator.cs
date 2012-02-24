using System;
using PassFruit.Contracts;

namespace PassFruit.Tests.FakeData {

    public class FakeDataGenerator {

        public void GenerateFakeData(IRepository repository) {

            var facebookAccount = repository.Accounts.Create();
            facebookAccount.Notes = @"Example test note 1";
            facebookAccount.SetField(FieldTypeName.Email, @"testFacebook@example.com");
            facebookAccount.AddTag("Tag 1");
            facebookAccount.AddTag("Tag 2");
            facebookAccount.SetPassword("Password1");
            repository.Accounts.Add(facebookAccount);

            var twitterAccount = repository.Accounts.Create();
            twitterAccount.Notes = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet";
            twitterAccount.SetField(FieldTypeName.Email, @"testTwitter@example.com");
            twitterAccount.SetField(FieldTypeName.UserName, @"TwitterUser");
            twitterAccount.AddTag("Tag 1");
            twitterAccount.SetPassword("Password2!£!$!$%!$&!£&!");
            repository.Accounts.Add(twitterAccount);

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

            
            
            repository.Accounts[2].SetPassword("Password3");
            repository.Accounts[3].SetPassword("Password4");

            repository.Accounts[2].AddTag("Tag 2");
            repository.Accounts[3].AddTag("Tag 3");

            repository.SaveAll();

        }

    }
}
