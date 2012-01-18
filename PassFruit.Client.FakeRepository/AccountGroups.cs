using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.FakeRepository {
    
    public class AccountGroups : IAccountGroups {

        private readonly IRepository _repository;

        private static IList<IAccountGroup> _accountGroups;
        private static readonly object Locker = new object();

        public AccountGroups(IRepository repository) {
            _repository = repository;
        }

        public IList<IAccountGroup> GetAll() {
            if (_accountGroups == null) {
                GenerateFakeData();
            }
            return _accountGroups;

        }

        private void GenerateFakeData() {
            lock (Locker) {
                _accountGroups = new List<IAccountGroup> {
                    new AccountGroup(Guid.NewGuid(), _repository) {
                        Name = @"Group 1",
                        Description = @"Description informations for group 1"
                    },
                    new AccountGroup(Guid.NewGuid(), _repository) {
                        Name = @"Group 2",
                        Description = @"Description informations for group 2"
                    }
                };
            }
        }
    }
}
