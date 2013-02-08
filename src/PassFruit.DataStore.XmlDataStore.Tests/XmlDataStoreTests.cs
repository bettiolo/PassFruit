using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using PassFruit.DataStore.Tests;
using PassFruit.DataStore.Tests.FakeData;

namespace PassFruit.DataStore.XmlDataStore.Tests {

    [TestFixture]
    public class XmlDataStoreTests : DataStoreTestsBase {

        protected override IDataStore CreateDataStoreWithFakeData()
        {
            var xmlDataStore = CreateEmptyDataStore();
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(xmlDataStore);
            return xmlDataStore;
        }

        protected override IDataStore CreateEmptyDataStore() {
            var xDocFileName = Path.GetTempFileName();
            Func<XDocument> getXDoc = () => File.Exists(xDocFileName) ? XDocument.Load(xDocFileName) : new XDocument();
            Action<XDocument> saveXDoc = xdoc => xdoc.Save(xDocFileName);
            if (File.Exists(xDocFileName))
            {
                File.Delete(xDocFileName);
            }
            var configuration = new XmlDataStoreConfiguration(getXDoc, saveXDoc);
            return new XmlDataStore(configuration);
        }

    }

}
