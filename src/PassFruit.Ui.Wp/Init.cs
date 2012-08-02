using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using PassFruit.Contracts;
using PassFruit.DataStore;
using PassFruit.DataStore.Contracts;
using PassFruit.DataStore.InMemoryDataStore;
using PassFruit.DataStore.XmlDataStore;
using PassFruit.Ui.Wp.Controls;

namespace PassFruit.Ui.Wp {

    public class Init {

        public Init() {
            TiltEffect.TiltableItems.Add(typeof(SettingsItem));
        }

        public DataStores GetDataStores() {
            var dataStores = new DataStores();
            var xmlDataStoreConfiguration = new XmlDataStoreConfiguration(null, null);
            var xmlDataStore = new XmlDataStore(xmlDataStoreConfiguration);
            dataStores.AddDataStore(xmlDataStore);

            var inMemoryDatasToreConfiguration = new InMemoryDataStoreConfiguration();
            var inMemoryDataStore = new InMemoryDataStore();
            dataStores.AddDataStore(inMemoryDataStore);

            dataStores.SelectDataStore(inMemoryDataStore);
            return dataStores;
        }

        public static IDataStore GetDataStore() {
            IDataStore dataStore;
            var app = Application.Current as App;
            if (app != null) {
                dataStore = app.DataStores.GetSelectedDataStore();
            } else {
                var init = new Init();
                dataStore = init.GetDataStores().GetSelectedDataStore();
            }
            return dataStore;
        }

    }

}
