using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PassFruit;
using PassFruit.DataStore;
using PassFruit.DataStore.Tests.FakeData;
using PassFruit.DataStore.XmlDataStore;

namespace Passfruit.Ui.ConsoleApp {
    
    public class Main {

        public void Run() {
            Console.WriteLine("Passfruit");
            Console.WriteLine();

            const string xmlFilePath = @"C:\passfruit.xml";
            var fileExists = File.Exists(xmlFilePath);

            var xmlDataStoreConfiguration = new XmlDataStoreConfiguration(
                () => fileExists ? XDocument.Load(xmlFilePath) : new XDocument(),
                xDocument => xDocument.Save(xmlFilePath)
            );
            var xmlDataStore = new XmlDataStore(xmlDataStoreConfiguration);

            if (!fileExists) {
                var fakeDataGenerator = new FakeDataGenerator();
                fakeDataGenerator.GenerateFakeData(xmlDataStore);
            }

            var accounts = new Accounts(xmlDataStore);

            Console.WriteLine(string.Format("Accounts ({0}):", accounts.Count()));
            var i = 1;
            foreach (var account in accounts) {
                Console.WriteLine("  - " + account.GetAccountName());
                i++;
            }
            Console.WriteLine();

            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
        }

    }

}
