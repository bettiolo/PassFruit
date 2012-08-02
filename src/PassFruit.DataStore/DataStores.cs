using System.Collections.Generic;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore {
    
    public class DataStores {

        private readonly IList<IDataStore> _dataStores = new List<IDataStore>();

        private IDataStore _currentDataStore;

        public void AddDataStore(IDataStore dataStore) {
            _dataStores.Add(dataStore);
        }

        public IEnumerable<IDataStore> GetAvailableDataStores() {
            return _dataStores;
        }

        public void SelectDataStore(IDataStore dataStore) {
            _currentDataStore = dataStore;
        }

        public IDataStore GetSelectedDataStore() {
            return _currentDataStore;
        }

    }

}
