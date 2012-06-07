using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PassFruit.Contracts;

namespace PassFruit {

    public class Tags : ITags {

        private readonly IRepository _repository;

        private readonly List<ITag> _tags = new List<ITag>();

        internal Tags(IRepository repository) {
            _repository = repository;
            ReloadTags();
        }

        private void ReloadTags() {
            _tags.Clear();
            _tags.AddRange(_repository.Accounts.SelectMany(account => account.Tags).Distinct());
        }

        public ITag this[string key] {
            get {
                return this.FirstOrDefault(accountTag => 
                    accountTag.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool Contains(string key) {
            return _tags.Any(accountTag => accountTag.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<ITag> GetByAccountId(Guid accountId) {
            return this.Where(accountTag => accountTag.Accounts.Any(account => account.Id == accountId));
        }

        public IEnumerator<ITag> GetEnumerator() {
            return _tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public ITag Create(string key) {
            return new Tag(_repository) { Key = key };
        }

    }

}
