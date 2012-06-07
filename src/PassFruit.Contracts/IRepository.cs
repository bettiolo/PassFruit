using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IRepository {

        string Name { get; }

        string Description { get; }

        IAccounts Accounts { get; }

        ITags Tags { get; }

        IProviders Providers { get; }

        IFieldTypes FieldTypes { get; }

        string GetPassword(Guid accountId, string passwordKey);

        void SetPassword(Guid accountId, string password, string passwordKey);

        event EventHandler<RepositorySaveEventArgs> OnSaved;

        event EventHandler<RepositorySaveEventArgs> OnSaving;

        void SaveAll();

        void Save(IAccount account);
        
    }
}
