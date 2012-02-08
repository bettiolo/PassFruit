using System;
using PassFruit.Contracts;

namespace PassFruit.Accounts {

    public abstract class UserAndEmailAccountBase : AccountBase {

        public override string Account {
            get { return User + " - " + Email; } 
        }

        public string User { get; set; }

        public string Email { get; set; }

    }

}
