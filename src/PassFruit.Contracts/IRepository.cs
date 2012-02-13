using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IRepository {

        string Name { get; }

        string Description { get; }

        IAccounts Accounts { get; }

        IAccountTags AccountTags { get; }

        string GetPassword(Guid accountId);

        void SetPassword(Guid accountId, string password);

    }

}
