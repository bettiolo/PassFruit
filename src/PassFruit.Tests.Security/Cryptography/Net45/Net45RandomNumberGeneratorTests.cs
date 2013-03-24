using PassFruit.Security.Cryptography;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Cryptography.Net45
{
    public class Net45RandomNumberGeneratorTests : RandomNumberGeneratorTests
    {

        protected override RandomNumberGenerator CreateRandomNumberGenerator()
        {
            return new Net45RandomNumberGenerator();
        }

    }
}