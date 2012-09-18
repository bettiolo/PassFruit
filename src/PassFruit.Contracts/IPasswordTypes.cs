using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IPasswordTypes : IEnumerable<IPasswordType> {

        // IPassword CreatePassword(PasswordTypeKey key, object value, Guid? fieldId = null);

    }

}
