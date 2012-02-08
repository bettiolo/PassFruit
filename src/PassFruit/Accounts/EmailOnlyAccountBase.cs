using System;
using PassFruit.Contracts;

namespace PassFruit.Accounts {

    public abstract class EmailOnlyAccountBase : AccountBase {

        public override string Account { 
            get { return Email; } 
        }

        public string Email { get; set; }

    }

}
