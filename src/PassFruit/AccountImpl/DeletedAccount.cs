using System;
using PassFruit.Contracts;

namespace PassFruit.AccountImpl {
    
    public class DeletedAccount : Account {

        public DeletedAccount(IRepository repository, Guid? id = null) 
            : base(repository, id) {

        }

        public override string GetAccountName() {
            return "< deleted >";
        }

    }

}
