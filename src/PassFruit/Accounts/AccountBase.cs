using System;
using PassFruit.Contracts;


namespace PassFruit.Accounts {

    public abstract class AccountBase : IAccount {

        public Guid Id { get; set; }

        public abstract string Account { get; }

        public string Note { get; set; }

        public override string ToString() {
            return Account + " | " + Provider.Name;
        }

        public abstract IAccountProvider Provider { get; }

        public IAccountPassword GetPassword() {
            throw new NotImplementedException();
        }

    }

}


