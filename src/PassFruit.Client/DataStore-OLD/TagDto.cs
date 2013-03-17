using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Datastore {

    public class TagDto {

        private string _key;

        public string Key {
            get {
                return _key;
            }
            set {
                _key = value.ToLowerInvariant();
            }
        }

        public bool Equals(TagDto other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._key, _key);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TagDto)) return false;
            return Equals((TagDto) obj);
        }

        public override int GetHashCode() {
            return (_key != null ? _key.GetHashCode() : 0);
        }
    }

}
