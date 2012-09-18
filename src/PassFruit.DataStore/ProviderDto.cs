using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore {

    public class ProviderDto : IProviderDto {

        public string Key { get; set; }

        public string Name { get; set; }

        public bool HasEmail { get; set; }

        public bool HasUserName { get; set; }

        public bool HasPassword { get; set; }

        public string Url { get; set; }

    }

}
