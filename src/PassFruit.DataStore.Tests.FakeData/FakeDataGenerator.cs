using System;
using PassFruit.DataStore;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore.Tests.FakeData {

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
        public const string Note1 = @"Example test note 1";
        public const string Note2 = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet";

        public void GenerateFakeData(IDataStore dataStore) {

            var facebookAccount = GetFacebookAccount();
            dataStore.SaveAccountDto(facebookAccount);
            var facebookPassword = GetFacebookPassword();
            dataStore.SavePasswordDto(facebookAccount.Id, facebookPassword);

            var twitterAccount = GetTwitterAccount();
            dataStore.SaveAccountDto(twitterAccount);
            var twitterPassword = GetTwitterPassword();
            dataStore.SavePasswordDto(twitterAccount.Id, twitterPassword);

            var google1Account = GetGoogle1Account();
            dataStore.SaveAccountDto(google1Account);
            var google1Password = GetGoogle1Password();
            dataStore.SavePasswordDto(google1Account.Id, google1Password);

            var google2Account = GetGoogle2Account();
            dataStore.SaveAccountDto(google2Account);
            var google2Password = GetGoogle2Password();
            dataStore.SavePasswordDto(google2Account.Id, google2Password);

        }

        public static IAccountDto GetFacebookAccount() {
            var facebookAccount = new AccountDto { ProviderKey = FacebookProviderKey, Notes = Note1 };
            facebookAccount.Fields.Add(new FieldDto { FieldTypeKey = "email", Name = "E-Mail", Value = FacebookEmail });
            facebookAccount.Tags.Add(Tag1);
            facebookAccount.Tags.Add(Tag2);
            return facebookAccount;
        }

        public static IPasswordDto GetFacebookPassword() {
            return new PasswordDto { Name = "Password", Password = FacebookPassword };
        }

        public static IAccountDto GetTwitterAccount() {
            var twitterAccount = new AccountDto { ProviderKey = TwitterProviderKey, Notes = Note2 };
            twitterAccount.Fields.Add(new FieldDto { FieldTypeKey = "email", Name = "E-Mail", Value = TwitterEmail });
            twitterAccount.Fields.Add(new FieldDto { FieldTypeKey = "username", Name = "User name", Value = TwitterUserName });
            twitterAccount.Tags.Add(Tag1);
            return twitterAccount;
        }

        private IPasswordDto GetTwitterPassword() {
            return new PasswordDto { Name = "password", Password = TwitterPassword };
        }

        private IAccountDto GetGoogle1Account() {
            var google1Account = new AccountDto { ProviderKey = GoogleProviderKey };
            google1Account.Fields.Add(new FieldDto { FieldTypeKey = "email", Name = "E-Mail", Value = Google1Email });
            google1Account.Tags.Add(Tag1);
            google1Account.Tags.Add(Tag2);
            google1Account.Tags.Add(Tag3);
            return google1Account;
        }

        private IPasswordDto GetGoogle1Password() {
            return new PasswordDto { Name = "password", Password = Goggle1Password };
        }

        private IAccountDto GetGoogle2Account() {
            var google2Account = new AccountDto { ProviderKey = GoogleProviderKey };
            google2Account.Fields.Add(new FieldDto { FieldTypeKey = "email", Name = "E-Mail", Value = Google2Email });
            google2Account.Tags.Add(Tag2);
            google2Account.Tags.Add(Tag3);
            return google2Account;
        }

        private IPasswordDto GetGoogle2Password() {
            return new PasswordDto { Name = "Password", Password = Google2Password };
        }
    }
}
