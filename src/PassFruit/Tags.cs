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

        public ITag this[string name] {
            get {
                return this.FirstOrDefault(accountTag => 
                    accountTag.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool Contains(string name) {
            return _tags.Any(accountTag => accountTag.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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

        public ITag Create(string tagName) {
            return new Tag(_repository) { Name = tagName };
        }

    }

}
