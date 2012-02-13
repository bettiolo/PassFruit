using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IAccountTags : IList<IAccountTag> {

        IAccountTag this[Guid id] { get; }

        IAccountTag this[string name] { get; }

        IEnumerable<IAccountTag> GetByAccountId(Guid accountId);

    }

}
