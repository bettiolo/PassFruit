using PassFruit.AccountProviders;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public class GoogleAccount : EmailOnlyAccountBase {

        public GoogleAccount(IRepository repository) : base(repository) {

        }

        public override IAccountProvider Provider {

            get {               
                return new GoogleAccountProvider();
            }

        }

    }
}
