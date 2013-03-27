using PassFruit.Security;
using PassFruit.Security.Cryptography;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Net45
{
    public class Net45AuthorizationTests : AuthorizationTests
    {

        protected override Pbkdf2 CreatePbkdf2()
        {
            return new Net45Pbkdf2();
        }

        protected override Aes CreateAes()
        {
            return new Net45Aes();
        }

        protected override HmacSha256 CreateHmacSha256()
        {
            return new Net45HmacSha256();
        }

    }
}
