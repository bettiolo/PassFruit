using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.FakeRepository {
    
    public class AccountLabels : IAccountLabels {

        private readonly IRepository _repository;

        private static IList<IAccountLabel> _accountLabels;
        private static readonly object Locker = new object();

        public AccountLabels(IRepository repository) {
            _repository = repository;
        }

        public IList<IAccountLabel> GetAll() {
            if (_accountLabels == null) {
                GenerateFakeData();
            }
            return _accountLabels;

        }

        private void GenerateFakeData() {
            lock (Locker) {
                _accountLabels = new List<IAccountLabel> {
                    new AccountLabel(Guid.NewGuid(), _repository) {
                        Name = @"Label 1",
                        Description = @"Description informations for Label 1"
                    },
                    new AccountLabel(Guid.NewGuid(), _repository) {
                        Name = @"Label 2",
                        Description = @"Description informations for Label 2"
                    }
                };
            }
        }
    }
}
