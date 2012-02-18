using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {
    
    public class DeletedAccount : AccountBase {

        public DeletedAccount(IRepository repository, Guid? id = null) 
            : base(repository, id) {

        }

        public override string AccountName {
            get { return "< deleted >"; }
        }

        public override IAccountProvider Provider {
            get { return null; }
        }
    }

}
