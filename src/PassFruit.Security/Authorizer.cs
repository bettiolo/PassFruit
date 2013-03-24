using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Security.Cryptography;

namespace PassFruit.Security
{
    public class Authorizer
    {
        private readonly Pbkdf2 _pbkdf2;
        private readonly Aes _aes;
        private readonly HmacSha256 _hmacSha256;

        private const string AuthorizedMessage = "AUTHORIZED";

        public Authorizer(Pbkdf2 pbkdf2, Aes aes, HmacSha256 hmacSha256)
        {
            _pbkdf2 = pbkdf2;
            _aes = aes;
            _hmacSha256 = hmacSha256;
        }

        public byte[] ComputeKey(string password, byte[] salt, int iterations)
        {
            var key = _pbkdf2.Compute(password, salt, iterations);
            return key;
        }

        public bool Authorize(byte[] key, AuthorizationDto authorization)
        {
            var hmac = CreateAuthorizationHmac(key, authorization.InitializationVector);
            return hmac.SequenceEqual(authorization.Hmac);
        }

        public AuthorizationDto CreateAuthorization(byte[] key, int iterations)
        {
            var salt = _pbkdf2.GenerateSalt();
            var initializationVector = _aes.GenerateInitializationVector();
            var hmac = CreateAuthorizationHmac(key, initializationVector);

            return new AuthorizationDto
            {
                Salt = salt,
                Iterations = iterations,
                InitializationVector = initializationVector,
                Hmac = hmac
            };
        }

        private byte[] CreateAuthorizationHmac(byte[] key, byte[] initializationVector)
        {
            var ciphertext = _aes.Encrypt(AuthorizedMessage, key, initializationVector);
            var hmac = _hmacSha256.Compute(ciphertext, key);
            return hmac;
        }

    }
}
