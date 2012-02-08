using PassFruit.AccountProviders;
using PassFruit.Contracts;

namespace PassFruit.Accounts {

    public class TwitterAccount : UserAndEmailAccountBase {

        public override IAccountProvider Provider {

            get {               
                return new TwitterAccountProvider();
            }

        }

    }
}
