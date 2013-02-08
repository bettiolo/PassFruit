using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.DataStore.JsonDataStore
{
    public class JsonAccounts
    {
        public JsonAccounts()
        {
            Accounts = new Dictionary<Guid, string>();
        }

        public IDictionary<Guid, string> Accounts { get; set; }
    }
}
