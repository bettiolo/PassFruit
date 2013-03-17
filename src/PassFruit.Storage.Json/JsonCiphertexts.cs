using System;
using System.Collections.Generic;

namespace PassFruit.Server.CiphertextDatastore.Json
{
    public class JsonCiphertexts
    {
        public JsonCiphertexts()
        {
            Ciphertexts = new Dictionary<Guid, string>();
        }

        public IDictionary<Guid, string> Ciphertexts { get; set; }
    }
}
