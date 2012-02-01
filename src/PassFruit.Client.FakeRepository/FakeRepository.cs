using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.FakeRepository {

    public class FakeRepository : IRepository {
        
        public string Name {
            get { return "Fake Repository"; }
        }

        public string Description {
            get { return "Fake Repository that returns fixed example data"; }
        }

        public IAccountLabels AccountLabels {
            get { return new AccountLabels(this); }
        }

        public IAccounts Accounts {
            get { return new Accounts(this); }
        }
    }

}
