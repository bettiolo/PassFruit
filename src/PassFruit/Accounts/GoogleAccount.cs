using PassFruit.AccountProviders;
using PassFruit.Contracts;

namespace PassFruit.Accounts {

    public class GoogleAccount : EmailOnlyAccountBase {

        public override IAccountProvider Provider {

            get {               
                return new GoogleAccountProvider();
            }

        }

    }
}
