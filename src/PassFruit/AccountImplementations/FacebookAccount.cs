using PassFruit.AccountProviders;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public class FacebookAccount : EmailOnlyAccountBase {

        public FacebookAccount(IRepository repository) : base(repository) {

        }

        public override IAccountProvider Provider {

            get {               
                return new FacebookAccountProvider();
            }

        }

    }
}
