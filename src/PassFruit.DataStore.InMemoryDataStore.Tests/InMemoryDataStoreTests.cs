using System.IO;
using NUnit.Framework;
using PassFruit.DataStore.Tests;
using PassFruit.DataStore.Tests.FakeData;
using System.Linq;

namespace PassFruit.DataStore.InMemoryDataStore.Tests {

    [TestFixture]
    public class InMemoryDataStoreTests : DataStoreTestsBase {

        protected override IDataStore CreateDataStoreWithFakeData() {
            var inMemoryDataStore = new InMemoryDataStore();
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(inMemoryDataStore);
            return inMemoryDataStore;
        }

        protected override IDataStore CreateEmptyDataStore() {
            return new InMemoryDataStore();
        }
    }

}
