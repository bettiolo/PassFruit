using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Accounts;
using PassFruit.Contracts;

namespace PassFruit.Client.FakeRepository {

    public class Accounts : IAccounts {

        private static readonly object Locker = new object();

        private readonly IRepository _repository;

        private static IList<IAccount> _accounts;
        private static IList<IAccountPassword> _password;
        private static IDictionary<Guid, IList<IAccount>> _labelsAccountsAssociation;
        private static IList<IAccountLabel> _accountLabels;

        public Accounts(IRepository repository) {
            _repository = repository;
            GenerateFakeData();
        }

        public IList<IAccount> GetAll() {
            return _accounts;
        }

        private void GenerateFakeData() {
            if (_accounts != null) {
                return;
            }
            lock (Locker) {
                _accounts = new List<IAccount> {
                    new FacebookAccount() {
                        Id = Guid.NewGuid(),
                        Notes = @"Example test note 1",
                        Email = @"test@example.com"
                    },
                    new TwitterAccount() {
                        Id = Guid.NewGuid(),
                        Notes = @"Example test note 2 blah blah blah\nBlah blah blah\nLorem ipsun dolor sit amet",
                        Email = @"test2@example.com",
                        User = @"TwitterUser"
                    },
                    new GoogleAccount() {
                        Id = Guid.NewGuid(),
                        Email = "example@gmail.com",
                        Notes = ""
                    },
                    new GoogleAccount() {
                        Id = Guid.NewGuid(),
                        Email = "example2@gmail.com",
                        Notes = ""
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
                    ,
                    new AccountPassword {
                        AccountId = _accounts[3].Id,
                        Password = @"Password4"
                    }
                };
                _accountLabels = new AccountLabels(_repository).GetAll();
                _labelsAccountsAssociation = new Dictionary<Guid, IList<IAccount>> {
                    { _accountLabels[0].Id, new List<IAccount> { _accounts[0], _accounts[1] }}, 
                    { _accountLabels[1].Id, new List<IAccount> { _accounts[0], _accounts[2] }}, 
                };
            }
        }

        public IAccountPassword GetPassword(IAccount account) {
            return _password.First(password => password.AccountId == account.Id);
        }

        public IList<IAccount> GetByAccountLabel(Guid accountLabelId) {
            return _labelsAccountsAssociation[accountLabelId];
        }

        public IAccount GetById(Guid accountId) {
            return GetAll().Single(account => account.Id == accountId);
        }
    }

}
