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

        public IProvider GetByKey(string providerKey) {
            var provider = _providers.SingleOrDefault(p => p.Key.Equals(providerKey, StringComparison.OrdinalIgnoreCase));
            return provider;
        }

        public void Add(string key, string name, bool hasEmail, bool hasUserName, bool hasPassword, string url) {
            var provider = new Provider(key) {
                Name = name,
                HasEmail = hasEmail,
                HasPassword = hasPassword,
                HasUserName = hasUserName,
                Url = url
            };
            _providers.Add(provider);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

}
