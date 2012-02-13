using System;
using System.Collections.Generic;

namespace PassFruit.Contracts {

    public interface IAccount {

        Guid Id { get; set; }

        string AccountName { get; }

        string Notes { get; set; }

        IAccountProvider Provider { get; }

        string GetPassword();

        void SetPassword(string password);

        IList<IAccountTag> AccountTags { get; }

        void AddTag(string tagName);

        void Save();
    }

}