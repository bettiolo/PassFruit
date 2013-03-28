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
        private readonly JsonCiphertexts _jsonCiphertexts;

        public JsonCiphertextDatastore(Guid userId, Func<string> load, Action<string> persist) : base(userId)
        {
            _jsonCiphertexts = JsonConvert.DeserializeObject<JsonCiphertexts>(load()) 
                ?? new JsonCiphertexts();
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

        public override Guid[] GetAllIds()
        {
            return _jsonCiphertexts.Ciphertexts.Keys;
        }

        public override CiphertextDto Get(Guid accountId)
        {
            return _jsonCiphertexts.Ciphertexts.ContainsKey(accountId) 
                ? JsonConvert.DeserializeObject<CiphertextDto>(_jsonCiphertexts.Ciphertexts[accountId]) 
                : null;
        }

        protected override void InternalSave(CiphertextDto ciphertextDto)
        {
            _jsonCiphertexts.Ciphertexts[ciphertextDto.Id] = JsonConvert.SerializeObject(ciphertextDto);
            lock (this)
            {
                _persist(JsonConvert.SerializeObject(_jsonCiphertexts));
            }
        }

    }
}
