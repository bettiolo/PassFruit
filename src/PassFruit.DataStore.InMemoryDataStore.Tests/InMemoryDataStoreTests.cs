using System.IO;
using NUnit.Framework;
using PassFruit.DataStore.Contracts;
using PassFruit.DataStore.Tests;
using PassFruit.DataStore.Tests.FakeData;

namespace PassFruit.DataStore.InMemoryDataStore.Tests {

    [TestFixture]
    public class InMemoryDataStoreTests : DataStoreTestsBase {

        private readonly InMemoryDataStoreConfiguration _configuration = 
            new InMemoryDataStoreConfiguration(Path.GetTempFileName());

        protected override IDataStore GetDataStoreWithFakeData() {
            var inMemoryDataStore = new InMemoryDataStore(_configuration);
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(inMemoryDataStore);
            return inMemoryDataStore;
        }

        protected override IDataStore GetDataStore() {
            return new InMemoryDataStore(_configuration);
        }
    }

}
