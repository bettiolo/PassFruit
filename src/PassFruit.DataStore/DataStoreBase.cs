using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore {

    public abstract class DataStoreBase : IDataStore {

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract IEnumerable<Guid> GetAllAccountIds();

        public IEnumerable<IAccountDto> GetAccountDtos(AccountDtoStatus accountStatus = AccountDtoStatus.Active) {
            return GetAllAccountIds().Select(GetAccountDto).Where(accountDto => 
                IsAccountDtoMatchedByStatus(accountStatus, accountDto.IsDeleted));
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

        private bool IsAccountDtoMatchedByStatus(AccountDtoStatus accountDtoStatus, bool isDeleted) {
            switch (accountDtoStatus) {
                case AccountDtoStatus.All:
                    return true;
                case AccountDtoStatus.Active:
                    return !isDeleted;
                case AccountDtoStatus.Deleted:
                    return isDeleted;
                default:
                    throw new NotSupportedException("The account status filter specified is not supported: " + accountDtoStatus);
            }
        }

    }

}
