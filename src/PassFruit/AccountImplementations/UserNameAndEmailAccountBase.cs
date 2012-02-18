using System;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public abstract class UserNameAndEmailAccountBase : AccountBase, IAccountWithEmail, IAccountWithUserName {

        protected UserNameAndEmailAccountBase(IRepository repository, Guid? id = null)
            : base(repository, id) {

        }

        public override string AccountName {
            get { return UserName + " - " + Email; } 
        }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool Equals(UserNameAndEmailAccountBase other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.UserName, UserName) && Equals(other.Email, Email);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as UserNameAndEmailAccountBase);
        }

        public override int GetHashCode() {
            unchecked {
                int result = base.GetHashCode();
                result = (result*397) ^ (UserName != null ? UserName.GetHashCode() : 0);
                result = (result*397) ^ (Email != null ? Email.GetHashCode() : 0);
                return result;
            }
        }

    }

}
