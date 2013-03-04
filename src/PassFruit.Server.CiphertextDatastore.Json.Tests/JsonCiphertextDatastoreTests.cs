using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassFruit.Server.CiphertextDatastore.Tests;
using PassFruit.Server.FakeDataGenerator;

namespace PassFruit.Server.CiphertextDatastore.Json.Tests
{

    public class JsonCiphertextDatastoreTests : CiphertextDatastoreTestBase
    {

        private string _tempFilePath;

        protected override CiphertextDatastoreBase CreateEmpty()
        {
            _tempFilePath = Path.GetTempFileName();
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
            return CreateDatastoreFromTempFile();
        }

        protected override CiphertextDatastoreBase CreatePopulatedWithFakeData()
        {
            var jsonCiphertextDatastore = CreateEmpty();
            CiphertextFakeDataGenerator.Populate(jsonCiphertextDatastore);
            return jsonCiphertextDatastore;
        }

        protected override CiphertextDatastoreBase CreateReloadedPopulatedWithFakeData()
        {
            var originalPopulatedCiphertextDatastore = CreatePopulatedWithFakeData(); // we ignore the original instance
            return CreateDatastoreFromTempFile();
        }

        private JsonCiphertextDatastore CreateDatastoreFromTempFile()
        {
            Debug.Print("Using text file for json persistence: " + _tempFilePath);

            return new JsonCiphertextDatastore(Guid.NewGuid(),
                () => LoadDatastore(_tempFilePath),
                json => SaveDatastore(_tempFilePath, json));
        }


        private void SaveDatastore(string path, string json)
        {
            File.WriteAllText(path, json);
        }

        private string LoadDatastore(string path)
        {
            if (!File.Exists(path))
            {
                return "";
            }
            return File.ReadAllText(path);
        }

    }
}
