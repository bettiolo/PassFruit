using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using PassFruit.DataStore.JsonDataStore;
using PassFruit.DataStore.Tests.FakeData;

namespace PassFruit.DataStore.Tests.JsonDataStore
{

    [TestFixture]
    public class JsonDataStoreTests : DataStoreTestsBase
    {

        protected override IDataStore CreateDataStoreWithFakeData()
        {
            var jsonDataStore = CreateEmptyDataStore();
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(jsonDataStore);
            return jsonDataStore;
        }

        protected override IDataStore CreateEmptyDataStore()
        {
            var tempFileName = Path.GetTempFileName();
            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
            Debug.Print("Using text file for json persistence: " + tempFileName);
            var configuration = new JsonDataStoreConfiguration(json => SaveDataStore(tempFileName, json));
            return new DataStore.JsonDataStore.JsonDataStore(configuration);
        }

        private void SaveDataStore(string fileName, string json)
        {
            File.WriteAllText(fileName, json);
        }

    }

}
