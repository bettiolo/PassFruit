using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class Passwords : IPasswords {

        private readonly IDataMapper _dataMapper;

        public Passwords(IDataMapper dataMapper) {
            _dataMapper = dataMapper;
        }

        public string GetPassword(Guid accountId, string passwordKey = "") {
            throw new NotImplementedException();
        }

        public void SetPassword(Guid accountId, string password, string passwordKey = "") {
            throw new NotImplementedException();
        }

        public void DeleteAccountPasswords(Guid accountId) {
            throw new NotImplementedException();
        }
    }

}
