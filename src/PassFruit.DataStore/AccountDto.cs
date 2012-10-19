using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace PassFruit.DataStore {

    public class AccountDto {

        public AccountDto() {
            Fields = new List<FieldDto>();
            Tags = new List<TagDto>();
            Notes = "";
        }

        public Guid Id { get; set; }

        public string ProviderKey { get; set; }

        public bool IsDeleted { get; set; }

        public IList<FieldDto> Fields { get; set; }

        public IList<TagDto> Tags { get; set; }

        public DateTime LastChangedUtc { get; set; }

        public string Notes { get; set; }

        public bool Equals(AccountDto otherAccountDto) {
            if (ReferenceEquals(null, otherAccountDto)) return false;
            if (ReferenceEquals(this, otherAccountDto)) return true;
            return otherAccountDto.ProviderKey.Equals(ProviderKey)
                && otherAccountDto.IsDeleted.Equals(IsDeleted) 
                && otherAccountDto.Id.Equals(Id)
                && otherAccountDto.Fields.SequenceEqual(Fields)
                && otherAccountDto.Tags.SequenceEqual(Tags)
                && otherAccountDto.Notes.Equals(Notes);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(AccountDto)) return false;
            return Equals((AccountDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int result = (ProviderKey != null ? ProviderKey.GetHashCode() : 0);
                result = (result * 397) ^ IsDeleted.GetHashCode();
                result = (result * 397) ^ Id.GetHashCode();
                result = (result * 397) ^ (Fields != null ? Fields.GetHashCode() : 0);
                result = (result * 397) ^ (Tags != null ? Tags.GetHashCode() : 0);
                result = (result * 397) ^ (Notes != null ? Notes.GetHashCode() : 0);
                return result;
            }
        }

        public override string ToString()
        {
            return Id.ToString() + " " + ProviderKey;
        }

    }

}
