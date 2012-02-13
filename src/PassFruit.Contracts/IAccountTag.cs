using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IAccountTag {

        Guid Id { get; }

        string Name { get; set; }

        string Description { get; set; }

        IEnumerable<IAccount> Accounts { get; }

    }

}
