using PassFruit.Security.Cryptography;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Cryptography
{
    public class Net45AesTests : AesTests
    {

        protected override Aes CreateAes()
        {
            return new Net45Aes();
        }

    }
}
