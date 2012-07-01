using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IRepository {

        string Name { get; }

        string Description { get; }

        IRepositoryConfiguration Configuration { get; }

        IAccounts Accounts { get; }

        IEnumerable<ITag> GetAllTags();

        IProviders Providers { get; }

        IFieldTypes FieldTypes { get; }

        string GetPassword(Guid accountId, string passwordKey = null);

        void SetPassword(Guid accountId, string password, string passwordKey = null);

        event EventHandler<RepositorySaveEventArgs> OnSaved;

        event EventHandler<RepositorySaveEventArgs> OnSaving;

        void SaveAll();

        void Save(IAccount account);

        bool IsDirty();

        void DeletePasswords(Guid accountId);

        IEnumerable<IAccount> GetDeletedAccounts();

    }
}
