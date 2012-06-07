using System;
using System.Collections.Generic;

namespace PassFruit.Contracts {

    public interface IAccount {

        Guid Id { get; }

        string EncryptionSalt { get; set; }

        string GetAccountName();

        string Notes { get; set; }

        IProvider Provider { get; }

        string GetPassword(string passwordKey = "");

        void SetPassword(string password, string passwordKey = "");

        IEnumerable<ITag> Tags { get; }

        IEnumerable<IField> Fields { get; }

        bool IsDirty { get; }

        void Save();

        void SetClean();

        IEnumerable<IField> GetFieldsByKey(FieldTypeKey fieldTypeKey);

        IField GetDefaultField(FieldTypeKey fieldTypeKey);
        
        void SetField(FieldTypeKey fieldTypeKey, object value);

        void AddTag(string tagName);

    }

}