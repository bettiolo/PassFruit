using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore {

    public class PasswordDto : IPasswordDto {

        public PasswordDto(Guid? passwordId = null) {
            Id = passwordId == null
                ? Guid.NewGuid()
                : passwordId.Value;
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public DateTime LastChangedUtc { get; set; }

        public bool Equals(IPasswordDto other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id) && Equals(other.Name, Name) && Equals(other.Password, Password);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(IPasswordDto)) return false;
            return Equals((IPasswordDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int result = Id.GetHashCode();
                result = (result * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                result = (result * 397) ^ (Password != null ? Password.GetHashCode() : 0);
                return result;
            }
        }

    }

}
