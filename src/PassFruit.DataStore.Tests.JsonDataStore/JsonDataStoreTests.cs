using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using PassFruit.Datastore.JsonDatastore;
using PassFruit.Datastore.Tests.FakeData;

namespace PassFruit.Datastore.Tests.JsonDatastore
{

    [TestFixture]
    public class JsonDatastoreTests : DatastoreTestsBase
    {

        protected override IDatastore CreateDatastoreWithFakeData()
        {
            var jsonDatastore = CreateEmptyDatastore();
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(jsonDatastore);
            return jsonDatastore;
        }

        protected override IDatastore CreateEmptyDatastore()
        {
            var tempFileName = Path.GetTempFileName();
            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
            Debug.Print("Using text file for json persistence: " + tempFileName);
            var configuration = new JsonDatastoreConfiguration(json => SaveDatastore(tempFileName, json));
            return new Datastore.JsonDatastore.JsonDatastore(configuration);
        }

        private void SaveDatastore(string fileName, string json)
        {
            File.WriteAllText(fileName, json);
        }

    }

}
