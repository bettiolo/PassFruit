using System;
using System.Xml.Linq;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore.XmlDataStore {
    
    public class XmlDataStoreConfiguration : IDataStoreConfiguration {

        public XmlDataStoreConfiguration(Func<XDocument> getXDoc, Action<XDocument> saveXdoc) {
            GetXDoc = getXDoc;
            SaveXdoc = saveXdoc;
        }

        /*
            var xmlFile = new FileInfo(_configuration.XmlFilePath);
            if (xmlFile.Exists && xmlFile.Length > 0) {
                _xDoc = XDocument.Load(xmlFile.FullName);
            } else {
                _xDoc = new XDocument();
            }
         */

        public Func<XDocument> GetXDoc { get; set; }

        public Action<XDocument> SaveXdoc { get; set; }

    }

}
