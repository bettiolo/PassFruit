using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore {

    public abstract class DataStoreBase : IDataStore {

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract IEnumerable<Guid> GetActiveAccountIds();

        public abstract IEnumerable<Guid> GetDeletedAccountIds();

        public IEnumerable<IAccountDto> GetActiveAccountDtos() {
            return GetActiveAccountIds().Select(GetAccountDto);
        }

        public abstract IAccountDto GetAccountDto(Guid accountId);

        public abstract void SaveAccountDto(IAccountDto accountDto);

        public void DeleteAccount(Guid accountId) {
            var deletedAccountDto = new AccountDto(accountId) { IsDeleted = true };
            DeleteAccountPasswords(accountId);
            SaveAccountDto(deletedAccountDto);
        }

        public abstract IEnumerable<IPasswordDto> GetPasswordDtos(Guid accountId);

        public abstract void SavePasswordDto(Guid accountId, IPasswordDto passwordDto);

        public abstract void DeleteAccountPasswords(Guid accountId);

    }

}
