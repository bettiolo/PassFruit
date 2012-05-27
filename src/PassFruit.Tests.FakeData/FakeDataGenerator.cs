using System;
using PassFruit.Contracts;

namespace PassFruit.Tests.FakeData {

    public class FakeDataGenerator {

        public void GenerateFakeData(IRepository repository) {

            var facebookAccount = repository.Accounts.Create("facebook");
            facebookAccount.Notes = @"Example test note 1";
            facebookAccount.SetField(FieldTypeKey.Email, @"testFacebook@example.com");
            facebookAccount.AddTag("Tag 1");
            facebookAccount.AddTag("Tag 2");
            facebookAccount.SetPassword("Password1");
            repository.Accounts.Add(facebookAccount);

            var twitterAccount = repository.Accounts.Create("twitter");
            twitterAccount.Notes = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet";
            twitterAccount.SetField(FieldTypeKey.Email, @"testTwitter@example.com");
            twitterAccount.SetField(FieldTypeKey.UserName, @"TwitterUser");
            twitterAccount.AddTag("Tag 1");
            twitterAccount.SetPassword("Password2!£!$!$%!$&!£&!");
            repository.Accounts.Add(twitterAccount);

            var googleAccount = repository.Accounts.Create("google");
            googleAccount.SetField(FieldTypeKey.Email, "user.name.example@gmail.com");
            googleAccount.AddTag("Tag 1");
            googleAccount.AddTag("Tag 2");
            googleAccount.AddTag("Tag 3");
            googleAccount.SetPassword("Password3");
            repository.Accounts.Add(googleAccount);

            var googleAccount2 = repository.Accounts.Create("google");
            googleAccount2.SetField(FieldTypeKey.Email, "example2@gmail.com");
            googleAccount2.AddTag("Tag 2");
            googleAccount2.AddTag("Tag 3");
            googleAccount2.SetPassword("Password4");
            repository.Accounts.Add(googleAccount2);
            
            repository.SaveAll();
        }

    }
}
