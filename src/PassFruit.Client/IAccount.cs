using System;
using System.Collections.Generic;

namespace PassFruit.Contracts {

    public interface IAccount {

        Guid Id { get; }

        string EncryptionSalt { get; set; }

        string GetAccountName();

        string Notes { get; set; }

        IProvider Provider { get; }

        ITags Tags { get; }

        IField[] Fields { get; }

        bool IsDirty { get; }

        IField[] GetFieldsByKey(FieldTypeKey fieldTypeKey);
        
        void SetField(FieldTypeKey fieldTypeKey, object value);

        DateTime LastChangedUtc { get; }

    }

}