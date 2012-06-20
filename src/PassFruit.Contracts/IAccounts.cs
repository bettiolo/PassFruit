using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IAccounts : IList<IAccount> {

        IAccount this[Guid accountId] { get; }

        IEnumerable<IAccount> GetByEmail(string email);

        IEnumerable<IAccount> GetByUserName(string userName);

        IAccount Create(string providerKey, Guid? id = null);

    }

}
