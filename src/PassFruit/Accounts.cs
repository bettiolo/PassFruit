using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PassFruit.Contracts;

namespace PassFruit {

    public class Accounts : Collection<IAccount>, IAccounts {

        private readonly IRepository _repository;

        public Accounts(IRepository repository) : base() {
            _repository = repository;
        }

        public IAccount this[Guid accountId] {
            get { return this.Single(account => account.Id == accountId); }
        }
    }

}
