using System;
using PassFruit.Contracts;

namespace PassFruit {
    
    public class DeletedAccount : Account {

        public DeletedAccount(IRepository repository, Guid? id = null) 
            : base(repository, null, id) {

        }

        public override string GetAccountName() {
            return "< deleted >";
        }

    }

}
