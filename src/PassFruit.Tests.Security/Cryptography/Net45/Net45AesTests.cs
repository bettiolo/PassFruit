using PassFruit.Security.Cryptography;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Cryptography.Net45
{
    public class Net45AesTests : AesTests
    {

        protected override Aes CreateAes()
        {
            return new Net45Aes();
        }

        protected override Pbkdf2 CreatePbkdf2()
        {
            return new Net45Pbkdf2();
        }

    }
}
