﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IProviders : IEnumerable<IProvider> {

        IProvider GetByKey(string providerKey);

        void Add(string key, string name, bool hasEmail, bool hasUserName, bool hasPassword, string url);
    
    }

}
