using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit {

    public class Account : IAccount {

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public IAccountPassword GetPassword() {
            throw new NotImplementedException();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Note { get; set; }

        public override string ToString() {
            return Name + " - " + Url;
        }

    }

}
