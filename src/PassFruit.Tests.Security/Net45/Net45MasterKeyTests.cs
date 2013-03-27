using PassFruit.Security.Cryptography;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Net45
{
    public class Net45MasterKeyTests : MasterKeyTests
    {
        protected override Pbkdf2 CreatePbkdf2()
        {
            return new Net45Pbkdf2();
        }
    }
}