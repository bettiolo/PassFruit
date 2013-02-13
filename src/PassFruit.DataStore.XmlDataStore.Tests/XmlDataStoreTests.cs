using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using PassFruit.Datastore.Tests;
using PassFruit.Datastore.Tests.FakeData;

namespace PassFruit.Datastore.XmlDatastore.Tests {

    [TestFixture]
    public class XmlDatastoreTests : DatastoreTestsBase {

        protected override IDatastore CreateDatastoreWithFakeData()
        {
            var xmlDatastore = CreateEmptyDatastore();
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(xmlDatastore);
            return xmlDatastore;
        }

        protected override IDatastore CreateEmptyDatastore() {
            var xDocFileName = Path.GetTempFileName();
            Func<XDocument> getXDoc = () => File.Exists(xDocFileName) ? XDocument.Load(xDocFileName) : new XDocument();
            Action<XDocument> saveXDoc = xdoc => xdoc.Save(xDocFileName);
            if (File.Exists(xDocFileName))
            {
                File.Delete(xDocFileName);
            }
            var configuration = new XmlDatastoreConfiguration(getXDoc, saveXDoc);
            return new XmlDatastore(configuration);
        }

    }

}
