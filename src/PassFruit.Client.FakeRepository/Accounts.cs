using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.FakeRepository {

    public class Accounts : IAccounts {

        private static readonly object Locker = new object();

        private readonly IRepository _repository;

        private static IList<IAccount> _accounts;
        private static IList<IAccountPassword> _password;
        private static IDictionary<Guid, IList<IAccount>> _tagsAccountsAssociation;
        private static IList<IAccountTag> _accountTags;

        public Accounts(IRepository repository) {
            _repository = repository;
        }

        public IList<IAccount> GetAll() {
            if (_accounts == null) {
                GenerateFakeData();
            }
            return _accounts;
        }

        private void GenerateFakeData() {
            lock (Locker) {
                _accounts = new List<IAccount> {
                    new Account {
                        Id = Guid.NewGuid(),
                        Name = @"Test 1",
                        Note = @"Example test note 1",
                        Url = @"http://www.example.com",
                        UserName = @"testuser"
                    },
                    new Account {
                        Id = Guid.NewGuid(),
                        Name = @"Test 2",
                        Note = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet",
                        Url = @"http://www.example.it",
                        UserName = @"testuser"
                    },
                    new Account {
                        Id = Guid.NewGuid(),
                        Name = @"Test website 3",
                        Note = @"",
                        Url = @"http://www.testsite.net",
                        UserName = @"testuser12313"
                    }
                };
                _password = new List<IAccountPassword> {
                    new AccountPassword {
                        AccountId = _accounts[0].Id,
                        Password = @"Password1"
                    },
                    new AccountPassword {
                        AccountId = _accounts[1].Id,
                        Password = @"Password2!£!$!$%!$&!£&!"
                    },
                    new AccountPassword {
                        AccountId = _accounts[2].Id,
                        Password = @"Password3"
                    }
                };
                _accountTags = new AccountTags(_repository).GetAll();
                _tagsAccountsAssociation = new Dictionary<Guid, IList<IAccount>> {
                    { _accountTags[0].Id, new List<IAccount> { _accounts[0], _accounts[1] }}, 
                    { _accountTags[1].Id, new List<IAccount> { _accounts[0], _accounts[2] }}, 
                };
            }
        }

        public IAccountPassword GetPassword(IAccount account) {
            if (_password == null) {
                GenerateFakeData();
            }
            return _password.First(password => password.AccountId == account.Id);
        }

        public IList<IAccount> GetByAccountTag(Guid accountTagId) {
            if (_tagsAccountsAssociation == null) {
                GenerateFakeData();
            }
            return _tagsAccountsAssociation[accountTagId];
        }
    }

}
