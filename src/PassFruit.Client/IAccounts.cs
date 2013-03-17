using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IAccounts : IEnumerable<IAccount> {

        IAccount this[Guid accountId] { get; }

        IEnumerable<IAccount> GetByEmail(string email);

        IEnumerable<IAccount> GetByUserName(string userName);

        IEnumerable<IAccount> GetByTag(string tagKey); 
            
        IAccount Create(string providerKey, Guid? id = null);

        IAccount GetById(Guid accountId);

        void Remove(Guid accountId);
    }

}
