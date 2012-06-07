using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface ITags : IEnumerable<ITag> {

        ITag this[string key] { get; }

        IEnumerable<ITag> GetByAccountId(Guid accountId);

        bool Contains(string key);

        ITag Create(string key);

    }

}
