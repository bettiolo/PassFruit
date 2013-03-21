using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class Ciphertext
    {

        internal Ciphertext(byte[] value, Salt salt, InitializationVector initializationVector)
        {
            Value = value;
            Salt = salt;
            InitializationVector = initializationVector;
        }

        public byte[] Value { get; private set; }

        public Salt Salt { get; private set; }

        public InitializationVector InitializationVector { get; private set; }
    
    }
}
