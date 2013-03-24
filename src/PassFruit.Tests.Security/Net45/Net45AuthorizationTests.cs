using PassFruit.Security;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Net45
{
    public class Net45AuthorizationTests : AuthorizationTests
    {

        protected override Authorizer CreateAuthorizer()
        {
            return new Authorizer(new Net45Pbkdf2(), new Net45Aes(), new Net45HmacSha256());
        }

    }
}
