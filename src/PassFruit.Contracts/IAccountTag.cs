using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IAccountTag {

        string Name { get; set; }

        IEnumerable<IAccount> Accounts { get; }

    }

}
