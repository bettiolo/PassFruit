using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PassFruit;
using PassFruit.Contracts;
using PassFruit.DataStore;
using PassFruit.DataStore.Tests.FakeData;
using PassFruit.DataStore.XmlDataStore;

namespace Passfruit.Ui.ConsoleApp {
    
    public class Main {

        private readonly XmlDataStore _dataStore;

        public Main() {
            // const string xmlFileName = @"passfruit.xml";
            var xmlFilePath = Path.GetTempFileName();
            File.Delete(xmlFilePath);
            var fileExists = File.Exists(xmlFilePath);

            var xmlDataStoreConfiguration = new XmlDataStoreConfiguration(
                () => fileExists ? XDocument.Load(xmlFilePath) : new XDocument(),
                xDocument => xDocument.Save(xmlFilePath)
            );
            _dataStore = new XmlDataStore(xmlDataStoreConfiguration);

            if (!fileExists) {
                var fakeDataGenerator = new FakeDataGenerator();
                fakeDataGenerator.GenerateFakeData(_dataStore);
            }

        }

        public void Run() {
            "Passfruit".Message();

            //do {
            //    var confirm =
            //        "Test Confirm"
            //            .Option("y", () => "Confirmed".Message())
            //            .Confirm();

            //    if (confirm) {
            //        "OK".Message();
            //    } else {
            //        "KO".Message();
            //    }

            //    "Options in Loop"
            //        .Option("a", () => "Option A".Message())
            //        .Option("b", () => "Option B".Message())
            //        .Option("c", () => "Option C".Message())
            //        .ChooseAndLoopOptions();

            // var selected = ...
            //    "Single option"
            //        .Option("a", () => "Option A".Message())
            //        .Option("b", () => "Option B".Message())
            //        .Option("c", () => "Option C".Message())
            //        .Choose(true);
            //} while (true);

            // .Options

            var accountSection = new AccountSection(_dataStore);
            do {
                accountSection.ChooseAccount();
            } while (!Exit());
        }

        private bool Exit() {
            return
            "Press (Y) to exit:"
                .Option("y", () => "Good bye".Message())
                .Confirm();
        }

    }

}
