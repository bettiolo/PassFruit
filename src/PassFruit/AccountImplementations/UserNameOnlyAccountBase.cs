using System;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public abstract class UserNameOnlyAccountBase : AccountBase, IAccountHasUserName {

        protected UserNameOnlyAccountBase(IRepository repository) : base(repository) {
        
        }

        public override string AccountName {
            get { return UserName; }
        }

        public string UserName { get; set; }

    }

}
