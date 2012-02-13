using PassFruit.AccountProviders;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public class TwitterAccount : UserNameAndEmailAccountBase {

        public TwitterAccount(IRepository repository) : base(repository) {

        }

        public override IAccountProvider Provider {

            get {               
                return new TwitterAccountProvider();
            }

        }

    }
}
