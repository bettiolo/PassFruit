using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PassFruit.Client.XmlRepository {

    public class XmlRepository : RepositoryBase {

        /*  <passfruit>
         *      <accounts>
         *          <0000-0000-0000-0000>
         *              <password>
         *              </password>
         *          </0000-0000-0000-0000>
         *      </accounts>
         *  </passfruit>    
         */

        public XmlRepository(string xmlFilePath) {
            _xmlFilePath = xmlFilePath;
        }

        private string _xmlFilePath;

        public override string Name {
            get { return "XML Repository"; }
        }

        public override string Description {
            get { return "XML repository, the data is persisted in a file"; }
        }

        public override string GetPassword(Guid accountId) {
            var xml = XDocument.Load(_xmlFilePath);
            var accountElement = xml.Elements(accountId.ToString()).FirstOrDefault();
            if (accountElement != null) {
                var passwordElement = accountElement.Element("password");
                if (passwordElement != null) {
                    return passwordElement.Value;
                }
            }
            return "";
        }

        public override void SetPassword(Guid accountId, string password) {
            throw new NotImplementedException();
        }

        protected override void LoadAllAccounts() {
            // Accounts.Add(Accounts.Create());
        }

        protected override void LoadAllAccountsExceptDeleted() {
            // Accounts.Add(Accounts.Create());
        }

        protected override void LoadAllAccountProviders() {
            Providers.Add("generic", "Generic", true, true, false, "");
            Providers.Add("facebook", "Facebook", true, false, false, "");
            Providers.Add("twitter", "Twitter", true, true, false, "");
            Providers.Add("google", "Google", true, false, false, "");
        }

        protected override void LoadAllFieldTypes() {
            // FieldTypes.Add(FieldTypes.Create());
        }
    }

}
