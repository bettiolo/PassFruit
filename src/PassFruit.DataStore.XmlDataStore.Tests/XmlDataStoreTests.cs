using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using PassFruit.DataStore.Tests;
using PassFruit.DataStore.Tests.FakeData;

namespace PassFruit.DataStore.XmlDataStore.Tests {

    [TestFixture]
    public class XmlDataStoreTests : DataStoreTestsBase {

        private readonly XmlDataStoreConfiguration _configuration;

        public XmlDataStoreTests() {
            var xDocFileName = Path.GetTempFileName();
            Func<XDocument> getXDoc = () => File.Exists(xDocFileName) ? XDocument.Load(xDocFileName) : new XDocument();
            Action<XDocument> saveXDoc = xdoc => xdoc.Save(xDocFileName);
            if (File.Exists(xDocFileName)) {
                File.Delete(xDocFileName);
            }
            _configuration = new XmlDataStoreConfiguration(getXDoc, saveXDoc);
        }

        protected override IDataStore GetDataStoreWithFakeData() {
            var xmlDataStore = new XmlDataStore(_configuration);
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(xmlDataStore);
            return xmlDataStore;
        }

        protected override IDataStore GetDataStore() {
            return new XmlDataStore(_configuration);
        }

    }

}
