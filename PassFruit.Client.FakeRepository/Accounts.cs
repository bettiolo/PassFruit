using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.FakeRepository {

    public class Accounts : IAccounts {

        private IList<IAccount> _accounts;
        private IList<IAccountPassword> _password;

        public IList<IAccount> GetAll() {
            if (_accounts == null) {
                GenerateFakeData();
            }
            return _accounts;
        }

        private void GenerateFakeData() {
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
        }

        public IAccountPassword GetPassword(IAccount account) {
            if (_password == null) {
                GenerateFakeData();
            }
            return _password.First(password => password.AccountId == account.Id);
        }
    }

}
