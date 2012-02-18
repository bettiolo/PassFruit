using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IAccounts : IList<IAccount> {

        IAccount this[Guid accountId] { get; }

        IEnumerable<IAccountWithEmail> GetByEmail(string email);

        IEnumerable<IAccountWithUserName> GetByUserName(string userName);

    }

}
