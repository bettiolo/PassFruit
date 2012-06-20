using PassFruit.Contracts;

namespace PassFruit.Client.InMemoryRepository {

    public class InMemoryRepositoryConfiguration : IRepositoryConfiguration {

        public InMemoryRepositoryConfiguration(string serializedFilePath) {
            _serializedFilePath = serializedFilePath;
        }

        private readonly string _serializedFilePath;

        public string SerializedFilePath {
            get { return _serializedFilePath; }
        }

    }

}