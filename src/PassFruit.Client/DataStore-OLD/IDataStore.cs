using System;
using System.Collections.Generic;
using PassFruit.Contracts;

namespace PassFruit.Datastore {

    public interface IDatastore {

        string Name { get; }

        string Description { get; }

        IEnumerable<Guid> GetAllAccountIds();

        IEnumerable<AccountDto> GetAccountDtos(AccountStatus accountStatus = AccountStatus.Active);

        AccountDto GetAccountDto(Guid accountId);

        void SaveAccountDto(AccountDto accountDto);

        void DeleteAccountDto(Guid accountId);

        void DeleteAllAccountDtos();

    }

}
