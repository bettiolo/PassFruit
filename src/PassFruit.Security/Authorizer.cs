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

        public bool Authorize(string password, AuthorizationDto authorization)
        {
            var hmac = CreateAuthorizationHmac(password, authorization.Salt,
                                               authorization.Iterations, authorization.InitializationVector);
            return hmac.SequenceEqual(authorization.Hmac);
        }

        public byte[] CreateAuthorizationHmac(string password, byte[] salt, int iterations, byte[] initializationVector)
        {
            var key = _pbkdf2.Compute(password, salt, iterations);
            var message = _aes.Encrypt(AuthorizedMessage, key, initializationVector);
            var hmac = _hmacSha256.Compute(message, key);
            return hmac;
        }

        public AuthorizationDto CreateAuthorization(string password, int iterations)
        {
            var salt = _pbkdf2.GenerateSalt();
            var initializationVector = _aes.GenerateInitializationVector();
            var hmac = CreateAuthorizationHmac(password, salt, iterations, initializationVector);

            return new AuthorizationDto
            {
                Salt = salt,
                Iterations = iterations,
                InitializationVector = initializationVector,
                Hmac = hmac
            };
        }

    }
}
