using System.IO;
using NUnit.Framework;
using PassFruit.Datastore.Tests;
using PassFruit.Datastore.Tests.FakeData;
using System.Linq;

namespace PassFruit.Datastore.InMemoryDatastore.Tests {

    [TestFixture]
    public class InMemoryDatastoreTests : DatastoreTestsBase {

        protected override IDatastore CreateDatastoreWithFakeData() {
            var inMemoryDatastore = new InMemoryDatastore();
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(inMemoryDatastore);
            return inMemoryDatastore;
        }

        protected override IDatastore CreateEmptyDatastore() {
            return new InMemoryDatastore();
        }
    }

}
