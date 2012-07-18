using System;
using System.Collections.Generic;

namespace PassFruit.DataStore.Contracts {

    public interface IAccountDto {

        Guid Id { get; }

        string ProviderKey { get; set; }

        bool IsDeleted { get; set; }

        IList<IFieldDto> Fields { get; set; }

        IList<string> Tags { get; set; }

        string Notes { get; set; }

        DateTime LastChangedUtc { get; set; }

        bool Equals(IAccountDto otherAccountDto);

    }

}
