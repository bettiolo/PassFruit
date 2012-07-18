using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PassFruit.Contracts;

namespace PassFruit {

    public class Tags : ITags {

        private readonly List<ITag> _tags = new List<ITag>();

        private readonly Guid _accountId;

        internal Tags(Guid accountId) {
            _accountId = accountId;
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

        public void Add(string key) {
            if (Contains(key)) {
                throw new Exception(string.Format("The tag '{0}' has already been added to the list", key));
            }
            _tags.Add(new Tag(key));
        }

        public void Remove(string key) {
            if (!Contains(key)) {
                throw new Exception(string.Format("The tag '{0}' has not been found in the list", key));
            }
            _tags.Remove(this[key]);
        }

        public IEnumerator<ITag> GetEnumerator() {
            return _tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public bool Equals(Tags other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._accountId.Equals(_accountId) 
                && Equals(other._tags, _tags);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Tags)) return false;
            return Equals((Tags) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (_accountId.GetHashCode() * 397) 
                    ^ (_tags != null ? _tags.GetHashCode() : 0);
            }
        }

    }

}
