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

        IList<ITag> Tags { get; }

        IList<IField> Fields { get; }

        bool IsDirty { get; }

        void Save();

        void SetClean();

        IEnumerable<IField<string>> GetFieldsByType(FieldTypeName fieldTypeName);

        IField<string> GetDefaultField(FieldTypeName fieldTypeName);
    }

}