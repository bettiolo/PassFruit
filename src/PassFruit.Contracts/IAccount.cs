using System;
using System.Collections.Generic;

namespace PassFruit.Contracts {

    public interface IAccount {

        Guid Id { get; }

        string EncryptionSalt { get; set; }

        string GetAccountName();

        string Notes { get; set; }

        IProvider Provider { get; }

        IEnumerable<IPassword> GetPasswords();

        void SetPassword(string password, string passwordKey = "");

        void DeleteAllPasswords();

        ITags Tags { get; }

        IEnumerable<IField> Fields { get; }

        bool IsDirty { get; }

        // void SetClean();

        IEnumerable<IField> GetFieldsByKey(FieldTypeKey fieldTypeKey);

        // IField GetDefaultField(FieldTypeKey fieldTypeKey);
        
        void SetField(FieldTypeKey fieldTypeKey, object value);

        DateTime LastChangedUtc { get; }

    }

}