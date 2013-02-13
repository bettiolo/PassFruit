using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Datastore {

    public abstract class DatastoreBase : IDatastore {

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract IEnumerable<Guid> GetAllAccountIds();

        public IEnumerable<AccountDto> GetAccountDtos(AccountStatus accountStatus = AccountStatus.Active) {
            return GetAllAccountIds().Select(GetAccountDto).Where(accountDto => 
                IsAccountDtoMatchedByStatus(accountStatus, accountDto.IsDeleted));
        }

        public abstract AccountDto GetAccountDto(Guid accountId);

        public void SaveAccountDto(AccountDto accountDto)
        {
            if (accountDto.IsNew)
            {
                accountDto.Id = Guid.NewGuid();
            }
            SaveSpecificAccountDto(accountDto);
        }

        protected abstract void SaveSpecificAccountDto(AccountDto accountDto);

        public void DeleteAccountDto(Guid accountId) {
            var deletedAccountDto = new AccountDto { Id = accountId, IsDeleted = true };
            SaveAccountDto(deletedAccountDto);
        }

        public void DeleteAllAccountDtos()
        {
            var allAccountIds = GetAllAccountIds();
            foreach (var accountId in allAccountIds)
            {
                DeleteAccountDto(accountId);
            }
        }

        private bool IsAccountDtoMatchedByStatus(AccountStatus accountStatus, bool isDeleted) {
            switch (accountStatus) {
                case AccountStatus.Any:
                    return true;
                case AccountStatus.Active:
                    return !isDeleted;
                case AccountStatus.Deleted:
                    return isDeleted;
                default:
                    throw new NotSupportedException("The account status filter specified is not supported: " + accountStatus);
            }
        }

    }

}
