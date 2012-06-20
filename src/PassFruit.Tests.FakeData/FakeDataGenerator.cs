using System;
using PassFruit.Contracts;

namespace PassFruit.Tests.FakeData {

    public class FakeDataGenerator {

        public const string FacebookEmail = @"testFacebook@example.com";
        public const string FacebookPassword = @"Password1";
        public const string TwitterEmail = @"testTwitter@example.com";
        public const string TwitterUserName = @"TwitterUser";
        public const string TwitterPassword = @"Password2!£!$!$%!$&!£&!";
        public const string Google1Email = @"user.name.example@gmail.com";
        public const string Goggle1Password = @"Password3";
        public const string Google2Email = @"example2@gmail.com";
        public const string Google2Password = @"Password4";

        public void GenerateFakeData(IRepository repository) {

            var facebookAccount = repository.Accounts.Create("facebook");
            facebookAccount.Notes = @"Example test note 1";
            facebookAccount.SetField(FieldTypeKey.Email, FacebookEmail);
            facebookAccount.AddTag("Tag 1");
            facebookAccount.AddTag("Tag 2");
            facebookAccount.SetPassword(FacebookPassword);
            repository.Accounts.Add(facebookAccount);

            var twitterAccount = repository.Accounts.Create("twitter");
            twitterAccount.Notes = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet";
            twitterAccount.SetField(FieldTypeKey.Email, TwitterEmail);
            twitterAccount.SetField(FieldTypeKey.UserName, TwitterUserName);
            twitterAccount.AddTag("Tag 1");
            twitterAccount.SetPassword(TwitterPassword);
            repository.Accounts.Add(twitterAccount);

            var googleAccount = repository.Accounts.Create("google");
            googleAccount.SetField(FieldTypeKey.Email, Google1Email);
            googleAccount.AddTag("Tag 1");
            googleAccount.AddTag("Tag 2");
            googleAccount.AddTag("Tag 3");
            googleAccount.SetPassword(Goggle1Password);
            repository.Accounts.Add(googleAccount);

            var googleAccount2 = repository.Accounts.Create("google");
            googleAccount2.SetField(FieldTypeKey.Email, Google2Email);
            googleAccount2.AddTag("Tag 2");
            googleAccount2.AddTag("Tag 3");
            googleAccount2.SetPassword(Google2Password);
            repository.Accounts.Add(googleAccount2);
            
            repository.SaveAll();
        }

    }
}
