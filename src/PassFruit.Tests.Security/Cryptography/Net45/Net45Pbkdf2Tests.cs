using PassFruit.Security.Cryptography;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Cryptography.Net45
{
    public class Net45Pbkdf2Tests : Pbkdf2Tests
    {

        protected override Pbkdf2 CreatePbkdf2()
        {
            return new Net45Pbkdf2();
        }

    }
}
