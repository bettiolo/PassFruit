using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class AccountPassword : IAccountPassword {
        
        public Guid AccountId { get; set; }

        public string Password { get; set; }
        }
    }

}
