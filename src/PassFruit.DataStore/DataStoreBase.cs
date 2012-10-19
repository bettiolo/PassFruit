using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.DataStore {

    public abstract class DataStoreBase : IDataStore {

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract IEnumerable<Guid> GetAllAccountIds();

        public IEnumerable<AccountDto> GetAccountDtos(AccountDtoStatus accountStatus = AccountDtoStatus.Active) {
            return GetAllAccountIds().Select(GetAccountDto).Where(accountDto => 
                IsAccountDtoMatchedByStatus(accountStatus, accountDto.IsDeleted));
        }

        public abstract AccountDto GetAccountDto(Guid accountId);

        public abstract void SaveAccountDto(AccountDto accountDto);

        public void DeleteAccountDto(Guid accountId) {
            var deletedAccountDto = new AccountDto {Id = accountId, IsDeleted = true };
            SaveAccountDto(deletedAccountDto);
        }

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
