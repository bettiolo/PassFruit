using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.AccountProviders {
    
    public class TwitterAccountProvider : AccountProviderBase {

        public override string Name {
            get { return "Twitter"; }
        }

        public override bool HasEmail {
            get { return true; }
        }

        public override bool HasUserName {
            get { return true; }
        }

        public override string Url {
            get { return "twitter.com"; }
        }

    }

}
