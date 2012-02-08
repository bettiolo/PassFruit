using PassFruit.Contracts;

namespace PassFruit.AccountProviders {

    public abstract class AccountProviderBase : IAccountProvider {

        public abstract string Name { get; }

        public abstract bool HasEmail { get; }

        public abstract bool HasUserName { get; }

        public abstract string Url { get; }

    }

}
