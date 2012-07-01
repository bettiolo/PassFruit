using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface ITags : IEnumerable<ITag> {

        ITag this[string key] { get; }

        bool Contains(string key);

        void Add(string key);

        void Remove(string key);

    }

}
