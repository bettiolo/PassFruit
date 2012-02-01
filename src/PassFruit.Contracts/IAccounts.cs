using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {
    
    public interface IAccounts {

        IList<IAccount> GetAll();

        IAccountPassword GetPassword(IAccount account);

        IList<IAccount> GetByAccountTag(Guid accountTagId);
    }

}
