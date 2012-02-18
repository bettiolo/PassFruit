using System;
using PassFruit.Contracts;

namespace PassFruit.AccountImplementations {

    public abstract class UserNameOnlyAccountBase : AccountBase, IAccountWithUserName {

        protected UserNameOnlyAccountBase(IRepository repository, Guid? id = null)
            : base(repository, id) {
        
        }

        public override string AccountName {
            get { return UserName; }
        }

        public string UserName { get; set; }

        public bool Equals(UserNameOnlyAccountBase other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.UserName, UserName);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as UserNameOnlyAccountBase);
        }

        public override int GetHashCode() {
            unchecked {
                return (base.GetHashCode()*397) ^ (UserName != null ? UserName.GetHashCode() : 0);
            }
        }

    }

}
