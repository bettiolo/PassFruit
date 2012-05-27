using System;
using System.Collections.Generic;

namespace PassFruit.Contracts {

    public interface IAccount {

        Guid Id { get; }

        string GetAccountName();

        string Notes { get; set; }

        IProvider Provider { get; }

        string GetPassword();

        void SetPassword(string password);

        IEnumerable<ITag> Tags { get; }

        IEnumerable<IField> Fields { get; }

        bool IsDirty { get; }

        void Save();

        void SetClean();

        IEnumerable<IField<TValue>> GetFieldsByKey<TValue>(FieldTypeKey fieldTypeKey);

        IField<TValue> GetDefaultField<TValue>(FieldTypeKey fieldTypeKey);
        
        void SetField<TValue>(FieldTypeKey fieldTypeKey, TValue value);

        void AddTag(string tagName);

    }

}