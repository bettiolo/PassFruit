using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface ITags : IEnumerable<ITag> {

        ITag this[string name] { get; }

        IEnumerable<ITag> GetByAccountId(Guid accountId);

        bool Contains(string name);

        ITag Create(string tagName);

    }

}
