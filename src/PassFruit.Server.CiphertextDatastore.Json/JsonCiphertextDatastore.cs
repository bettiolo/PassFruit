using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PassFruit.Server.CiphertextDatastore;

namespace PassFruit.Server.CiphertextDatastore.Json
{
    public class JsonCiphertextDatastore : CiphertextDatastoreBase
    {

        private readonly Action<string> _persist;
        private readonly JsonCipheredAccounts _jsonCipheredAccounts;

        public JsonCiphertextDatastore(Guid userId, Func<string> load, Action<string> persist) : base(userId)
        {
            _jsonCipheredAccounts = JsonConvert.DeserializeObject<JsonCipheredAccounts>(load()) 
                ?? new JsonCipheredAccounts();
            _persist = persist;
        }

        public override string Name
        {
            get { return "JSON ciphertext data store"; }
        }

        public override string Description
        {
            get { return "JSON ciphertext data store, the data is persisted in a json format usin JSON.Net"; }
        }

        public override IEnumerable<Guid> GetAllIds()
        {
            return _jsonCipheredAccounts.CipheredAccounts.Keys;
        }

        public override CipheredAccountDto Get(Guid accountId)
        {
            return _jsonCipheredAccounts.CipheredAccounts.ContainsKey(accountId) 
                ? JsonConvert.DeserializeObject<CipheredAccountDto>(_jsonCipheredAccounts.CipheredAccounts[accountId]) 
                : null;
        }

        protected override void InternalSave(CipheredAccountDto cipheredAccountDto)
        {
            _jsonCipheredAccounts.CipheredAccounts[cipheredAccountDto.Id] = JsonConvert.SerializeObject(cipheredAccountDto);
            lock (this)
            {
                _persist(JsonConvert.SerializeObject(_jsonCipheredAccounts));
            }
        }

    }
}
