using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IAccountWithEmail : IAccount {

        string Email { get; set; }

    }

}
