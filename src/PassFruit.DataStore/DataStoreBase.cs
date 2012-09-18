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

        public void DeleteAccountDto(Guid accountId) {
            var deletedAccountDto = new AccountDto(accountId) { IsDeleted = true };
            DeleteAccountPasswordDtos(deletedAccountDto);
            SaveAccountDto(deletedAccountDto);
        }

        public abstract IEnumerable<IPasswordDto> GetPasswordDtos(Guid accountId);

        public abstract void SavePasswordDtos(IAccountDto accountDto, IEnumerable<IPasswordDto> passwordDtos);

        public abstract void DeleteAccountPasswordDtos(IAccountDto accountDto);

        public abstract IEnumerable<IProviderDto> GetProviderDtos();

        public abstract void ReplaceProviderDtos(IEnumerable<IProviderDto> providerDtos);

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
