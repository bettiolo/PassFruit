using System;
using PassFruit.Datastore;

namespace PassFruit.Datastore.Tests.FakeData
{

    public class FakeDataGenerator
    {

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
        public const string Google2Email = @"example2@gmail.com";
        public const string Goggle2PasswordA = @"Password3";
        public const string Google2PasswordB = @"Password4";
        public const string Tag1 = @"Tag-1";
        public const string Tag2 = @"Tag-2";
        public const string Tag3 = @"Tag-3";
        public const string Note1 = @"Example test note 1";
        public const string Note2 = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet";

        public void GenerateFakeData(IDatastore dataStore)
        {

            var facebookAccount = GetFacebookAccount();
            dataStore.SaveAccountDto(facebookAccount);

            var twitterAccount = GetTwitterAccount();
            dataStore.SaveAccountDto(twitterAccount);

            var google1Account = GetGoogle1Account();
            dataStore.SaveAccountDto(google1Account);

            var google2Account = GetGoogle2Account();
            dataStore.SaveAccountDto(google2Account);

        }

        public static AccountDto GetFacebookAccount()
        {
            var facebookAccount = new AccountDto { ProviderKey = FacebookProviderKey, Notes = Note1 };
            facebookAccount.Fields.Add(new FieldDto {Id = Guid.NewGuid(), FieldTypeKey = "email", Name = "E-Mail", Value = FacebookEmail });
            facebookAccount.Fields.Add(GetFacebookPassword());
            facebookAccount.Tags.Add(new TagDto { Key = Tag1 });
            facebookAccount.Tags.Add(new TagDto { Key = Tag2 });
            return facebookAccount;
        }

        public static FieldDto GetFacebookPassword()
        {
            return new FieldDto {Id = Guid.NewGuid(), FieldTypeKey = "password", Name = "Password", Value = FacebookPassword };
        }

        public static AccountDto GetTwitterAccount()
        {
            var twitterAccount = new AccountDto { ProviderKey = TwitterProviderKey, Notes = Note2 };
            twitterAccount.Fields.Add(new FieldDto { Id = Guid.NewGuid(), FieldTypeKey = "email", Name = "E-Mail", Value = TwitterEmail });
            twitterAccount.Fields.Add(new FieldDto { Id = Guid.NewGuid(), FieldTypeKey = "username", Name = "User name", Value = TwitterUserName });
            twitterAccount.Fields.Add(GetTwitterPassword());
            twitterAccount.Tags.Add(new TagDto { Key = Tag1 });
            return twitterAccount;
        }

        public static FieldDto GetTwitterPassword()
        {
            return new FieldDto { Id = Guid.NewGuid(), FieldTypeKey = "pin", Name = "PIN", Value = TwitterPassword };
        }

        public AccountDto GetGoogle1Account()
        {
            var google1Account = new AccountDto { ProviderKey = GoogleProviderKey };
            google1Account.Fields.Add(new FieldDto { Id = Guid.NewGuid(), FieldTypeKey = "email", Name = "E-Mail", Value = Google1Email });
            google1Account.Tags.Add(new TagDto { Key = Tag1 });
            google1Account.Tags.Add(new TagDto { Key = Tag2 });
            google1Account.Tags.Add(new TagDto { Key = Tag3 });
            return google1Account;
        }

        public AccountDto GetGoogle2Account()
        {
            var google2Account = new AccountDto { ProviderKey = GoogleProviderKey };
            google2Account.Fields.Add(new FieldDto { Id = Guid.NewGuid(), FieldTypeKey = "email", Name = "E-Mail", Value = Google2Email });
            google2Account.Fields.Add(GetGoogle2PasswordA());
            google2Account.Fields.Add(GetGoogle2PasswordB());
            google2Account.Tags.Add(new TagDto { Key = Tag2 });
            google2Account.Tags.Add(new TagDto { Key = Tag3 });
            return google2Account;
        }

        public static FieldDto GetGoogle2PasswordA()
        {
            return new FieldDto { Id = Guid.NewGuid(), FieldTypeKey = "password", Name = "password 1", Value = Goggle2PasswordA };
        }

        public FieldDto GetGoogle2PasswordB()
        {
            return new FieldDto { Id = Guid.NewGuid(), FieldTypeKey = "password", Name = "Password 2", Value = Google2PasswordB };
        }
    }
}
