using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.XmlRepository {
    
    public class XmlRepositoryConfiguration : IRepositoryConfiguration {
        
        public XmlRepositoryConfiguration(string xmlFilePath) {
            _xmlFilePath = xmlFilePath;
        }

        private readonly string _xmlFilePath;

        public string XmlFilePath {
            get { return _xmlFilePath; }
        }

    }

}
