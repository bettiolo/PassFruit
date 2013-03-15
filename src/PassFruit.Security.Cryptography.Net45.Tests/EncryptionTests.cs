using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassFruit.Security.Cryptography.Net45.Tests
{
    public class EncryptionTests : AesTestsBase
    {

        protected override RandomNumberGeneratorBase CreateRandomNumberGenerator()
        {
            return new Net45RandomNumberGenerator();
        }

        protected override KeyGeneratorBase CreateKeyGenerator()
        {
            return new Net45KeyGenerator();
        }

        protected override AesBase CreateAes()
        {
            return new Net45Aes();
        }

    }
}
