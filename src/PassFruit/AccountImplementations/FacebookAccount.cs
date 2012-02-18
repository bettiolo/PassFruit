using System;
using PassFruit.AccountProviders;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public class FacebookAccount : EmailOnlyAccountBase {

        public FacebookAccount(IRepository repository, Guid? id = null)
            : base(repository, id) {

        }

        public override IAccountProvider Provider {

            get {               
                return new FacebookAccountProvider();
            }

        }

    }
}
