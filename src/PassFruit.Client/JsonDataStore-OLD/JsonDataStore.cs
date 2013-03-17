using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PassFruit.Datastore.JsonDatastore
{
    public class JsonDatastore : DatastoreBase
    {
        private readonly JsonDatastoreConfiguration _configuration;

        private string _serializedJsonAccounts = "";

        public JsonDatastore(JsonDatastoreConfiguration configuration)
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
            lock (this)
            {
                var jsonAccounts = JsonConvert.DeserializeObject<JsonAccounts>(_serializedJsonAccounts);
                return jsonAccounts ?? new JsonAccounts();
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

        protected override void SaveSpecificAccountDto(AccountDto accountDto)
        {
            var jsonAccounts = GetJsonAccounts();
            jsonAccounts.Accounts[accountDto.Id] = JsonConvert.SerializeObject(accountDto);
            lock (this)
            {
                _serializedJsonAccounts = JsonConvert.SerializeObject(jsonAccounts);
                _configuration.Persist(_serializedJsonAccounts);
            }
        }

    }

}