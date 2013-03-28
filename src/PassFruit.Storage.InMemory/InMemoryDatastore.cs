using System;
using System.Collections.Generic;
using System.Linq;

namespace PassFruit.Datastore.InMemoryDatastore {

    public class InMemoryDatastore : DatastoreBase {

        //private readonly InMemoryDatastoreConfiguration _configuration;

        //public InMemoryDatastore(InMemoryDatastoreConfiguration configuration) {
        //    _configuration = configuration;
        //}

        private readonly Dictionary<Guid, AccountDto> _accountDtos =
            new Dictionary<Guid, AccountDto>();

        public override string Name {
            get { return "In Memory Datastore"; }
        }

        public override string Description {
            get { return "In Memory Datastore, the data is serialized"; }
        }

        public override Guid[] GetAllAccountIds() {
            return _accountDtos.Select(item => item.Key).ToArray();
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
