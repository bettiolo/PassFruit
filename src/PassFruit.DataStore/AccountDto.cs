using System;
using System.Collections.Generic;
using PassFruit.DataStore.Contracts;
using System.Linq;
using System.Collections;

namespace PassFruit.DataStore {

    public class AccountDto : IAccountDto {

        public AccountDto(Guid? accuntId = null) {
            Id = accuntId == null 
                ? Guid.NewGuid() 
                : accuntId.Value;
            Fields = new List<IFieldDto>();
            Tags = new List<ITagDto>();
        }

        public Guid Id { get; private set; }

        public string ProviderKey { get; set; }

        public bool IsDeleted { get; set; }

        public IList<IFieldDto> Fields { get; set; }

        public IList<ITagDto> Tags { get; set; }

        public DateTime LastChangedUtc { get; set; }

        public string Notes { get; set; }

        public bool Equals(IAccountDto otherAccountDto) {
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
            if (obj.GetType() != typeof(IAccountDto)) return false;
            return Equals((IAccountDto)obj);
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

    }

}
