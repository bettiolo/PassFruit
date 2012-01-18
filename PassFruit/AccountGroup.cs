using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class AccountGroup : IAccountGroup {

        private IRepository _repository;
        
        private Guid _id;

        public AccountGroup( Guid id, IRepository repository) {
            _id = id;
            _repository = repository;
        }

        public Guid Id { get { return _id; } }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<IAccount> Accounts {
            get { return _repository.Accounts.GetByAccountGroup(Id); }
        }

    }
}
