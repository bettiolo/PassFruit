using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore {

    public class TagDto : ITagDto {

        private string _name;

        public string Name {
            get {
                return _name;
            }
            set {
                _name = value.ToLowerInvariant();
            }
        }

        public bool Equals(TagDto other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._name, _name);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TagDto)) return false;
            return Equals((TagDto) obj);
        }

        public override int GetHashCode() {
            return (_name != null ? _name.GetHashCode() : 0);
        }
    }

}
