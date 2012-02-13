using System;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public abstract class EmailOnlyAccountBase : AccountBase, IAccountHasEmail {

        protected EmailOnlyAccountBase(IRepository repository) : base(repository) {

        }

        public override string AccountName { 
            get { return Email; } 
        }

        public string Email { get; set; }

    }

}
