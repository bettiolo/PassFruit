using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IAccountTags : IEnumerable<IAccountTag> {

        IAccountTag this[string name] { get; }

        IEnumerable<IAccountTag> GetByAccountId(Guid accountId);

        bool Contains(string name);
    }

}
