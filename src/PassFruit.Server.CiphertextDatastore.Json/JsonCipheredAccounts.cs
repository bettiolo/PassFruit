using System;
using System.Collections.Generic;

namespace PassFruit.Server.CiphertextDatastore.Json
{
    public class JsonCipheredAccounts
    {
        public JsonCipheredAccounts()
        {
            CipheredAccounts = new Dictionary<Guid, string>();
        }

        public IDictionary<Guid, string> CipheredAccounts { get; set; }
    }
}
