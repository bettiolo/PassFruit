using System;
using PassFruit.Contracts;

namespace PassFruit.Accounts {

    public abstract class UserOnlyAccountBase : AccountBase {

        public override string Account {
            get { return User; }
        }

        public string User { get; set; }

    }

}
