using System;

namespace PassFruit.Contracts {

    public interface IPasswords {

        string GetPassword(Guid accountId, string passwordKey = "");

        void SetPassword(Guid accountId, string password, string passwordKey = "");

        void DeleteAccountPasswords(Guid accountId);

    }

}