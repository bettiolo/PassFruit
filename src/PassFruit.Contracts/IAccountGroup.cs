using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {
    public interface IAccountGroup {

        Guid Id { get; }

        string Name { get; set; }

        string Description { get; set; }

        IList<IAccount> Accounts { get; }

    }
}
