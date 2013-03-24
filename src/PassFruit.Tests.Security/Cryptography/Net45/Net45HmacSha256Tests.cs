using PassFruit.Security.Cryptography;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Cryptography.Net45
{
    public class Net45HmacSha256Tests : HmacSha256Tests
    {

        protected override HmacSha256 CreateHmacSha256()
        {
            return new Net45HmacSha256();
        }

    }
}