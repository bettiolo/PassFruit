using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IRepository {

        string Name { get; }

        string Description { get; }

        IAccountTags AccountTags { get; }

        IAccounts Accounts { get; }

    }

}
