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

        private readonly JsonDataStoreConfiguration _configuration;

        public JsonDataStoreTests()
        {
            var fileName = Path.GetTempFileName();
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            Debug.Print("Using text file for json persistence: " + fileName);
            _configuration = new JsonDataStoreConfiguration("", 
                () => File.WriteAllText(fileName, _configuration.JsonAccountsString));
        }

        protected override IDataStore GetDataStoreWithFakeData()
        {
            var xmlDataStore = new DataStore.JsonDataStore.JsonDataStore(_configuration);
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(xmlDataStore);
            return xmlDataStore;
        }

        protected override IDataStore GetDataStore()
        {
            return new DataStore.JsonDataStore.JsonDataStore(_configuration);
        }

    }

}
