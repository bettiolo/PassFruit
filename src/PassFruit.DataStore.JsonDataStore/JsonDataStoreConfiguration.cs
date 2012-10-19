using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.DataStore.JsonDataStore
{
    public class JsonDataStoreConfiguration : IDataStoreConfiguration
    {
        private readonly Action _save;

        public JsonDataStoreConfiguration(string jsonAccountsString, Action save)
        {
            _save = save;
            JsonAccountsString = jsonAccountsString;
        }

        public string JsonAccountsString { get; private set; }

        public void Update(string jsonAccountsString)
        {
            JsonAccountsString = jsonAccountsString;
            _save();
        }
    }
}
