using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class Providers : IProviders {

        private readonly IRepository _repository;

        private readonly IList<IProvider> _providers = new List<IProvider>();

        public Providers(IRepository repository) {
            _repository = repository;
        }

        public IEnumerator<IProvider> GetEnumerator() {
            return _providers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

}
