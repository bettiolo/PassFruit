using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Security
{
    public class AuthorizationDto
    {
        
        public byte[] Salt { get; set; }

        public int Iterations { get; set; }

        public byte[] InitializationVector { get; set; }

        public byte[] Hmac { get; set; }

    }
}
