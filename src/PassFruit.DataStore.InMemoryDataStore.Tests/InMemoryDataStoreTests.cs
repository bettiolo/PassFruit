using System.IO;
using NUnit.Framework;
using PassFruit.DataStore.Tests;
using PassFruit.DataStore.Tests.FakeData;
using System.Linq;

namespace PassFruit.DataStore.InMemoryDataStore.Tests {

    [TestFixture]
    public class InMemoryDataStoreTests : DataStoreTestsBase {

        private readonly InMemoryDataStoreConfiguration _configuration = 
            new InMemoryDataStoreConfiguration();

        protected override IDataStore GetDataStoreWithFakeData() {
            var inMemoryDataStore = _configuration.Instance;
            var fakeDataGenerator = new FakeDataGenerator();
            var deletedIds = inMemoryDataStore.GetAllAccountIds().ToArray();
            foreach (var accountId in deletedIds) {
                inMemoryDataStore.DeleteAccountDto(accountId);
            }
            fakeDataGenerator.GenerateFakeData(inMemoryDataStore);
            return inMemoryDataStore;
        }

        protected override IDataStore GetDataStore() {
            return _configuration.Instance;
        }
    }

}
