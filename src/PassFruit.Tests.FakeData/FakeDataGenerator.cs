using System;
using PassFruit.Contracts;

namespace PassFruit.Tests.FakeData {

    public class FakeDataGenerator {

        public const string GenericProviderKey = "generic";
        public const string FacebookProviderKey = "facebook";
        public const string TwitterProviderKey = "twitter";
        public const string GoogleProviderKey = "google";
        public const string FacebookEmail = @"testFacebook@example.com";
        public const string FacebookPassword = @"Password1";
        public const string TwitterEmail = @"testTwitter@example.com";
        public const string TwitterUserName = @"TwitterUser";
        public const string TwitterPassword = @"Password2!£!$!$%!$&!£&!";
        public const string Google1Email = @"user.name.example@gmail.com";
        public const string Goggle1Password = @"Password3";
        public const string Google2Email = @"example2@gmail.com";
        public const string Google2Password = @"Password4";
        public const string Tag1 = @"Tag-1";
        public const string Tag2 = @"Tag-2";
        public const string Tag3 = @"Tag-3";

        public void GenerateFakeData(IRepository repository) {

            var facebookAccount = repository.Accounts.Create(FacebookProviderKey);
            facebookAccount.Notes = @"Example test note 1";
            facebookAccount.SetField(FieldTypeKey.Email, FacebookEmail);
            facebookAccount.Tags.Add(Tag1);
            facebookAccount.Tags.Add(Tag2);
            facebookAccount.SetPassword(FacebookPassword);
            repository.Accounts.Add(facebookAccount);

            var twitterAccount = repository.Accounts.Create(TwitterProviderKey);
            twitterAccount.Notes = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet";
            twitterAccount.SetField(FieldTypeKey.Email, TwitterEmail);
            twitterAccount.SetField(FieldTypeKey.UserName, TwitterUserName);
            twitterAccount.Tags.Add(Tag1);
            twitterAccount.SetPassword(TwitterPassword);
            repository.Accounts.Add(twitterAccount);

            var googleAccount = repository.Accounts.Create(GoogleProviderKey);
            googleAccount.SetField(FieldTypeKey.Email, Google1Email);
            googleAccount.Tags.Add(Tag1);
            googleAccount.Tags.Add(Tag2);
            googleAccount.Tags.Add(Tag3);
            googleAccount.SetPassword(Goggle1Password);
            repository.Accounts.Add(googleAccount);

            var googleAccount2 = repository.Accounts.Create(GoogleProviderKey);
            googleAccount2.SetField(FieldTypeKey.Email, Google2Email);
            googleAccount2.Tags.Add(Tag2);
            googleAccount2.Tags.Add(Tag3);
            googleAccount2.SetPassword(Google2Password);
            repository.Accounts.Add(googleAccount2);
            
            repository.SaveAll();
        }

    }
}
