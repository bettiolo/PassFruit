using System.IO;
using NUnit.Framework;
using PassFruit.DataStore.Contracts;
using PassFruit.DataStore.Tests;
using PassFruit.DataStore.Tests.FakeData;

namespace PassFruit.DataStore.InMemoryDataStore.Tests {

    [TestFixture]
    public class InMemoryDataStoreTests : DataStoreTestsBase {

        private readonly InMemoryDataStoreConfiguration _configuration = 
            new InMemoryDataStoreConfiguration();

        protected override IDataStore GetDataStoreWithFakeData() {
            var inMemoryDataStore = _configuration.Instance;
            var fakeDataGenerator = new FakeDataGenerator();
            foreach (var accountId in inMemoryDataStore.GetAllAccountIds()) {
                inMemoryDataStore.DeleteAccount(accountId);
            }
            fakeDataGenerator.GenerateFakeData(inMemoryDataStore);
            return inMemoryDataStore;
        }

        protected override IDataStore GetDataStore() {
            return _configuration.Instance;
        }
    }

}
