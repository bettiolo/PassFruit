using System;
using System.Collections.Generic;

namespace PassFruit.Contracts {

    public interface IAccount {

        Guid Id { get; }

        string AccountName { get; }

        string Notes { get; set; }

        IAccountProvider Provider { get; }

        string GetPassword();

        void SetPassword(string password);

        IList<IAccountTag> AccountTags { get; }

        bool IsDirty { get; }

        void AddTag(string tagName);

        void Save();

        void SetClean();

    }

}