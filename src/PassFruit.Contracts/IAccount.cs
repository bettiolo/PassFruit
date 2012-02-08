using System;

namespace PassFruit.Contracts {

    public interface IAccount {

        Guid Id { get; set; }

        string Account { get; }

        string Note { get; set; }

        IAccountProvider Provider { get; }

        IAccountPassword GetPassword();

    }
}