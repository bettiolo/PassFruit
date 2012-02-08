using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.AccountProviders {
    
    public class GoogleAccountProvider : AccountProviderBase {

        public override string Name {
            get { return "Google"; }
        }

        public override bool HasEmail {
            get { return true; }
        }

        public override bool HasUserName {
            get { return false; }
        }

        public override string Url {
            get { return "google.com"; }
        }

    }

}
