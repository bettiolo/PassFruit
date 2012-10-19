using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PassFruit.DataStore.JsonDataStore
{
    public class JsonDataStore : DataStoreBase
    {
        private readonly JsonDataStoreConfiguration _configuration;
        private readonly object _locker = new object();

        public JsonDataStore(JsonDataStoreConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override string Name
        {
            get { return "JSON data store"; }
        }

        public override string Description
        {
            get { return "JSON data store, the data is persisted in a json format usin JSON.Net"; }
        }

        private JsonAccounts GetJsonAccounts()
        {
            lock (_locker)
            {
                return JsonConvert.DeserializeObject<JsonAccounts>(_configuration.JsonAccountsString);
            }
        }

        public override IEnumerable<Guid> GetAllAccountIds()
        {
            return GetJsonAccounts().Accounts.Keys;
        }

        public override AccountDto GetAccountDto(Guid accountId)
        {
            var jsonAccounts = GetJsonAccounts().Accounts;
            if (jsonAccounts.ContainsKey(accountId))
            {
                return JsonConvert.DeserializeObject<AccountDto>(jsonAccounts[accountId]);
            }
            return null;
        }

        public override void SaveAccountDto(AccountDto accountDto)
        {
            var jsonAccounts = GetJsonAccounts();
            if (jsonAccounts == null)
            {
                jsonAccounts = new JsonAccounts();
            }
            if (jsonAccounts.Accounts == null)
            {
                jsonAccounts.Accounts = new Dictionary<Guid, string>();
            }
            jsonAccounts.Accounts[accountDto.Id] = JsonConvert.SerializeObject(accountDto);
            lock (_locker)
            {
                _configuration.Update(JsonConvert.SerializeObject(jsonAccounts));
            }
        }

    }

}