using System;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public abstract class UserNameAndEmailAccountBase : AccountBase, IAccountHasEmail, IAccountHasUserName {

        protected UserNameAndEmailAccountBase(IRepository repository) : base(repository) {

        }

        public override string AccountName {
            get { return UserName + " - " + Email; } 
        }

        public string UserName { get; set; }

        public string Email { get; set; }

    }

}
