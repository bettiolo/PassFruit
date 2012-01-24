using System;

namespace PassFruit.Contracts {

    public interface IAccount {

        Guid Id { get; set; }

        string UserName { get; set; }

        string Name { get; set; }

        string Url { get; set; }

        string Note { get; set; }

        IAccountPassword GetPassword();

    }
}