using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class AccountLabel : IAccountLabel {

        private IRepository _repository;
        
        private Guid _id;

        public AccountLabel( Guid id, IRepository repository) {
            _id = id;
            _repository = repository;
        }

        public Guid Id { get { return _id; } }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<IAccount> Accounts {
            get { return _repository.Accounts.GetByAccountLabel(Id); }
        }

        public override string ToString() {
            return Name;
        }

    }
}
