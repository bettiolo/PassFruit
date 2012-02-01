using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.FakeRepository {
    
    public class AccountTags : IAccountTags {

        private readonly IRepository _repository;

        private static IList<IAccountTag> _accountTags;
        private static readonly object Locker = new object();

        public AccountTags(IRepository repository) {
            _repository = repository;
        }

        public IList<IAccountTag> GetAll() {
            if (_accountTags == null) {
                GenerateFakeData();
            }
            return _accountTags;

        }

        private void GenerateFakeData() {
            lock (Locker) {
                _accountTags = new List<IAccountTag> {
                    new AccountTag(Guid.NewGuid(), _repository) {
                        Name = @"Tag 1",
                        Description = @"Description informations for Tag 1"
                    },
                    new AccountTag(Guid.NewGuid(), _repository) {
                        Name = @"Tag 2",
                        Description = @"Description informations for Tag 2"
                    }
                };
            }
        }
    }
}
