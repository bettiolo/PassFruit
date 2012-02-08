using PassFruit.AccountProviders;
using PassFruit.Contracts;

namespace PassFruit.Accounts {

    public class FacebookAccount : EmailOnlyAccountBase {

        public override IAccountProvider Provider {

            get {               
                return new FacebookAccountProvider();
            }

        }

    }
}
