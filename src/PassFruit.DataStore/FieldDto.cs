using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore {

    public class FieldDto : IFieldDto {

        public FieldDto(Guid? id = null) {
            Id = id ?? Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public string FieldTypeKey { get; set; }

        public object Value { get; set; }

        public bool Equals(IFieldDto otherFieldDto) {
            if (ReferenceEquals(null, otherFieldDto)) return false;
            if (ReferenceEquals(this, otherFieldDto)) return true;
            return otherFieldDto.Id.Equals(Id) && Equals(otherFieldDto.Name, Name) && Equals(otherFieldDto.FieldTypeKey, FieldTypeKey) && Equals(otherFieldDto.Value, Value);
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
                result = (result * 397) ^ Name.GetHashCode();
                result = (result * 397) ^ FieldTypeKey.GetHashCode();
                result = (result * 397) ^ Value.GetHashCode();
                return result;
            }
        }
    }

}
