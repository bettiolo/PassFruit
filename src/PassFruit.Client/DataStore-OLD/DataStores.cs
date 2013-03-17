using System.Collections.Generic;

namespace PassFruit.Datastore {
    
    public class Datastores {

        private readonly IList<IDatastore> _dataStores = new List<IDatastore>();

        private IDatastore _currentDatastore;

        public void AddDatastore(IDatastore dataStore) {
            _dataStores.Add(dataStore);
        }

        public IEnumerable<IDatastore> GetAvailableDatastores() {
            return _dataStores;
        }

        public void SelectDatastore(IDatastore dataStore) {
            _currentDatastore = dataStore;
        }

        public IDatastore GetSelectedDatastore() {
            return _currentDatastore;
        }

    }

}
