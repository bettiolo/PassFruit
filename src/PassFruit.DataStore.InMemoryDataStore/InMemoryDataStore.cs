using System;
using System.Collections.Generic;
using System.Linq;

namespace PassFruit.DataStore.InMemoryDataStore {

    public class InMemoryDataStore : DataStoreBase {

        //private readonly InMemoryDataStoreConfiguration _configuration;

        //public InMemoryDataStore(InMemoryDataStoreConfiguration configuration) {
        //    _configuration = configuration;
        //}

        private readonly Dictionary<Guid, AccountDto> _accountDtos =
            new Dictionary<Guid, AccountDto>();

        public override string Name {
            get { return "In Memory DataStore"; }
        }

        public override string Description {
            get { return "In Memory DataStore, the data is serialized"; }
        }

        public override IEnumerable<Guid> GetAllAccountIds() {
            return _accountDtos.Select(item => item.Key);
        }

        public override AccountDto GetAccountDto(Guid accountId) {
            return _accountDtos[accountId];
        }

        protected override void SaveSpecificAccountDto(AccountDto accountDto) {
            if (_accountDtos.ContainsKey(accountDto.Id)
                && _accountDtos[accountDto.Id] != null
                && _accountDtos[accountDto.Id].Equals(accountDto)) {
                return;
            }
            accountDto.LastChangedUtc = DateTime.UtcNow;
            _accountDtos[accountDto.Id] = accountDto;
        }

    }

}
