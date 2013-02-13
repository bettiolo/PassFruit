using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Datastore {

    public class FieldDto {

        public FieldDto() {
            Properties = new Dictionary<string, object>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FieldTypeKey { get; set; }

        public object Value { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public bool Equals(FieldDto otherFieldDto) {
            if (ReferenceEquals(null, otherFieldDto)) return false;
            if (ReferenceEquals(this, otherFieldDto)) return true;
            return otherFieldDto.Id.Equals(Id)
                && Equals(otherFieldDto.Name, Name)
                && Equals(otherFieldDto.FieldTypeKey, FieldTypeKey)
                && Equals(otherFieldDto.Value, Value)
                && Properties.SequenceEqual(otherFieldDto.Properties);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (FieldDto)) return false;
            return Equals((FieldDto) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int result = Id.GetHashCode();
                result = (result * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                result = (result * 397) ^ (FieldTypeKey != null ? FieldTypeKey.GetHashCode() : 0);
                result = (result * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                result = (result * 397) ^ (Properties != null ? Properties.GetHashCode() : 0);
                return result;
            }
        }
    }

}
