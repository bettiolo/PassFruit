using System;
using System.Collections.Generic;
using System.Linq;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore.InMemoryDataStore {

    public class InMemoryDataStore : DataStoreBase {

        //private readonly InMemoryDataStoreConfiguration _configuration;

        //public InMemoryDataStore(InMemoryDataStoreConfiguration configuration) {
        //    _configuration = configuration;
        //}

        private readonly Dictionary<Guid, IAccountDto> _accountDtos =
            new Dictionary<Guid, IAccountDto>();

        private readonly Dictionary<Guid, Dictionary<Guid, IPasswordDto>> _passwordDtos =
            new Dictionary<Guid, Dictionary<Guid, IPasswordDto>>();

        private readonly Dictionary<Guid, IEnumerable<IFieldDto>> _fieldDtos =
            new Dictionary<Guid, IEnumerable<IFieldDto>>();

        public override string Name {
            get { return "In Memory DataStore"; }
        }

        public override string Description {
            get { return "In Memory DataStore, the data is serialized"; }
        }

        public override IEnumerable<IPasswordDto> GetPasswordDtos(Guid accountId) {
            return _passwordDtos[accountId] == null
                ? new List<IPasswordDto>()
                : _passwordDtos[accountId].Select(accountPasswordDto => accountPasswordDto.Value);
        }

        public override void SavePasswordDtos(IAccountDto accountDto, IEnumerable<IPasswordDto> passwordDtos) {
            var accountId = accountDto.Id;
            if (_passwordDtos.ContainsKey(accountId)
                    && _passwordDtos[accountId] != null) {
                if (_passwordDtos[accountId].Values.SequenceEqual(passwordDtos)) {
                    return;
                } else {
                    _passwordDtos[accountId].Clear();
                }
            }

            foreach (var passwordDto in passwordDtos) {
                passwordDto.LastChangedUtc = DateTime.UtcNow;
                if (!_passwordDtos.ContainsKey(accountId)
                    || _passwordDtos[accountId] == null) {
                    _passwordDtos.Add(accountId, new Dictionary<Guid, IPasswordDto>());
                }
                _passwordDtos[accountId][passwordDto.Id] = passwordDto;
            }
            accountDto.LastChangedUtc = DateTime.UtcNow;
        }

        public override void DeleteAccountPasswords(IAccountDto accountDto) {
            _passwordDtos.Remove(accountDto.Id);
            accountDto.LastChangedUtc = DateTime.UtcNow;
        }

        public override IEnumerable<Guid> GetAllAccountIds() {
            return _accountDtos.Select(item => item.Key);
        }

        public override IAccountDto GetAccountDto(Guid accountId) {
            return _accountDtos[accountId];
        }

        public override void SaveAccountDto(IAccountDto accountDto) {
            if (_accountDtos.ContainsKey(accountDto.Id)
                && _accountDtos[accountDto.Id] != null
                && _accountDtos[accountDto.Id].Equals(accountDto)) {
                return;
            }
            accountDto.LastChangedUtc = DateTime.UtcNow;
            _accountDtos[accountDto.Id] = accountDto;
        }

        public IEnumerable<IFieldDto> GetAccountFieldDtos(Guid accountId) {
            return _fieldDtos[accountId];
        }


    }

}
