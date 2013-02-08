using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.DataStore.JsonDataStore
{
    public class JsonDataStoreConfiguration : IDataStoreConfiguration
    {
        private readonly Action<string> _persist;

        public JsonDataStoreConfiguration(Action<string> persist)
        {
            _persist = persist;
        }

        public void Persist(string json)
        {
            _persist(json);
        }
    }
}
