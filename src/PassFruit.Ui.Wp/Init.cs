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
using PassFruit.Datastore;
using PassFruit.Datastore.Contracts;
using PassFruit.Datastore.InMemoryDatastore;
using PassFruit.Datastore.XmlDatastore;
using PassFruit.Ui.Wp.Controls;

namespace PassFruit.Ui.Wp {

    public class Init {

        public Init() {
            TiltEffect.TiltableItems.Add(typeof(SettingsItem));
        }

        public Datastores GetDatastores() {
            var dataStores = new Datastores();
            var xmlDatastoreConfiguration = new XmlDatastoreConfiguration(null, null);
            var xmlDatastore = new XmlDatastore(xmlDatastoreConfiguration);
            dataStores.AddDatastore(xmlDatastore);

            var inMemoryDatasToreConfiguration = new InMemoryDatastoreConfiguration();
            var inMemoryDatastore = new InMemoryDatastore();
            dataStores.AddDatastore(inMemoryDatastore);

            dataStores.SelectDatastore(inMemoryDatastore);
            return dataStores;
        }

        public static IDatastore GetDatastore() {
            IDatastore dataStore;
            var app = Application.Current as App;
            if (app != null) {
                dataStore = app.Datastores.GetSelectedDatastore();
            } else {
                var init = new Init();
                dataStore = init.GetDatastores().GetSelectedDatastore();
            }
            return dataStore;
        }

    }

}
