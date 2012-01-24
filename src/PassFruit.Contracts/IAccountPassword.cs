using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {
    
    public interface IAccountPassword {

        Guid AccountId { get; set; }

        string Password { get; set; }

    }
}
