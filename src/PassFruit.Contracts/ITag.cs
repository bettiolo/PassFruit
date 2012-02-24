using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface ITag {

        string Name { get; set; }

        IEnumerable<IAccount> Accounts { get; }

    }

}
