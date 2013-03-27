using PassFruit.Security;
using PassFruit.Security.Cryptography.Net45;

namespace PassFruit.Tests.Security.Net45
{
    public class Net45EncryptorTests : EncryptorTests
    {

        protected override MasterKey ComputeMasterKey(string secretPassword, byte[] salt, int iterations)
        {
            return new MasterKey(secretPassword, salt, iterations, new Net45Pbkdf2());
        }

        protected override Encryptor CreateEncryptor()
        {
            return new Encryptor(new Net45Pbkdf2(), new Net45Aes());
        }

    }
}