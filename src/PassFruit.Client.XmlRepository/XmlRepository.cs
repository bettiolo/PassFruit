using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PassFruit.Client.XmlRepository {

    public class XmlRepository : RepositoryBase {

        /*  <passfruit>
         *      <accounts>
         *          <0000-0000-0000-0000>
         *              <details>
         *                  <username>user name</username>
         *                  <email>info@example.org</email>
         *              </details>
         *          </0000-0000-0000-0000>
         *      </accounts>
         *      <passwords>
         *          <0000-0000-0000-0000>
         *              <default>
         *                  ENCRYPTED DATA
         *              </default>
         *      </passwords>
         *  </passfruit>    
         */

        public XmlRepository(string xmlFilePath) {
            _xmlFilePath = xmlFilePath;
            LoadXDocument();
        }

        private XDocument _xDoc;

        private readonly string _xmlFilePath;

        public override string Name {
            get { return "XML Repository"; }
        }

        public override string Description {
            get { return "XML repository, the data is persisted in a XML file"; }
        }

        public override string GetPassword(Guid accountId, string passwordKey = DefaultPasswordKey) {
            if (String.IsNullOrEmpty(passwordKey)) {
                passwordKey = DefaultPasswordKey;
            }
            var passwordElement = GetPasswordElement(accountId, passwordKey);
            return passwordElement == null ? "" : passwordElement.Value;
        }

        public override void SetPassword(Guid accountId, string password, string passwordKey = DefaultPasswordKey) {
            if (String.IsNullOrEmpty(passwordKey)) {
                passwordKey = DefaultPasswordKey;
            }
            var passwordElement = GetPasswordElement(accountId, passwordKey);
            if (passwordElement == null) {
                passwordElement = new XElement(passwordKey);
                var accountPasswordsElement = GetAccountPasswordsElement(accountId);
                accountPasswordsElement.Add(passwordElement);
            }
            passwordElement.Value = password;
            SaveXml();
        }

        protected override void LoadAllAccounts() {
            // Accounts.Add(Accounts.Create());
        }

        protected override void LoadAllAccountsExceptDeleted() {
            // Accounts.Add(Accounts.Create());
        }

        protected override void LoadAllFieldTypes() {
            // FieldTypes.Add(FieldTypes.Create());
        }

        private void LoadXDocument() {
            var xmlFile = new FileInfo(_xmlFilePath);
            
            if (xmlFile.Exists && xmlFile.Length > 0) {
                _xDoc = XDocument.Load(_xmlFilePath);
            } else {
                _xDoc = new XDocument();
            }
        }

        private XElement GetPassfruitElement() {
            const string passfruitElementName = "passfruit";
            var passfruitElement = _xDoc.Element(passfruitElementName);
            if (passfruitElement == null) {
                passfruitElement = new XElement(passfruitElementName);
                _xDoc.Add(passfruitElement);
            }
            return passfruitElement;
        }

        private XElement GetPasswordsElement() {
            const string passwordsElementName = "passwords";
            var passfruitElement = GetPassfruitElement();
            var passwordsElement = passfruitElement.Element(passwordsElementName);
            if (passwordsElement == null) {
                passwordsElement = new XElement(passwordsElementName);
                passfruitElement.Add(passwordsElement);
            }
            return passwordsElement;
        }

        private XElement GetAccountPasswordsElement(Guid accountId) {
            var accountIdElementName = "ID-" + accountId.ToString();
            var passwordsElement = GetPasswordsElement();
            var accountPasswordsElement = passwordsElement.Element(accountIdElementName);
            if (accountPasswordsElement == null) {
                accountPasswordsElement = new XElement(accountIdElementName);
                passwordsElement.Add(passwordsElement);
            }
            return accountPasswordsElement;
        }

        private XElement GetPasswordElement(Guid accountId, string passwordKey) {
            var accountPasswordsElement = GetAccountPasswordsElement(accountId);
            var passwordElement = accountPasswordsElement.Element(passwordKey);
            return passwordElement;
        }

        private void SaveXml() {
            _xDoc.Save(_xmlFilePath);
        }

    }

}
