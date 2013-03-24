using PassFruit.Security;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Net45
{
    public class Net45EncryptorTests : EncryptorTests
    {
        protected override Authorizer CreateAuthorizer()
        {
            return new Authorizer(new Net45Pbkdf2(), new Net45Aes(), new Net45HmacSha256());
        }

        protected override Encryptor CreateEncryptor()
        {
            return new Encryptor(new Net45Pbkdf2(), new Net45Aes());
        }
    }
}