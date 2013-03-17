using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Datastore.JsonDatastore
{
    public class JsonDatastoreConfiguration : IDatastoreConfiguration
    {
        private readonly Action<string> _persist;

        public JsonDatastoreConfiguration(Action<string> persist)
        {
            _persist = persist;
        }

        public void Persist(string json)
        {
            _persist(json);
        }
    }
}
