using System;
using System.Collections.Generic;

namespace PassFruit.DataStore {

    public interface IDataStore {

        string Name { get; }

        string Description { get; }

        IEnumerable<Guid> GetAllAccountIds();

        IEnumerable<AccountDto> GetAccountDtos(AccountDtoStatus accountStatus = AccountDtoStatus.Active);

        AccountDto GetAccountDto(Guid accountId);

        void SaveAccountDto(AccountDto accountDto);

        void DeleteAccountDto(Guid accountId);

        void DeleteAllAccountDtos();

    }

}
