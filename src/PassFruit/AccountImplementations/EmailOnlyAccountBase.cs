using System;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public abstract class EmailOnlyAccountBase : AccountBase, IAccountHasEmail {

        protected EmailOnlyAccountBase(IRepository repository) : base(repository) {

        }

        public override string AccountName { 
            get { return Email; } 
        }

        public string Email { get; set; }

        public bool Equals(EmailOnlyAccountBase other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.Email, Email);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as EmailOnlyAccountBase);
        }

        public override int GetHashCode() {
            unchecked {
                return (base.GetHashCode()*397) ^ (Email != null ? Email.GetHashCode() : 0);
            }
        }

    }

}
