using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.DataStore.Contracts {

    public interface IPasswordDto {

        Guid Id { get; }

        string Name { get; set; }

        string Password { get; set; }

        DateTime LastChangedUtc { get; set; }

        bool Equals(IPasswordDto other);

    }

}
