using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore.InMemoryDataStore {

    public class InMemoryDataStoreConfiguration : IDataStoreConfiguration {

        public InMemoryDataStoreConfiguration(string serializedFilePath) {
            _serializedFilePath = serializedFilePath;
        }

        private readonly string _serializedFilePath;

        public string SerializedFilePath {
            get { return _serializedFilePath; }
        }

    }

}