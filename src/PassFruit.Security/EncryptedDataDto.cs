using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security.Cryptography
{
    public class EncryptedDataDto
    {
        
        public byte[] Salt { get; set; }
        
        public byte[] InitializationVector { get; set; }
        
        public int Iterations { get; set; }

        public byte[] Ciphertext { get; set; }

    }
}
