using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.AccountProviders {
    
    public class FacebookAccountProvider : AccountProviderBase {

        public override string Name {
            get { return "Facebook"; }
        }

        public override bool HasEmail {
            get { return true; }
        }

        public override bool HasUserName {
            get { return false; }
        }

        public override string Url {
            get { return "facebook.com"; }
        }

    }

}
