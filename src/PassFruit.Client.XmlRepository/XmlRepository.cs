using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using PassFruit.Contracts;

namespace PassFruit.Client.XmlRepository {

    public class XmlRepository : RepositoryBase {

        /*  <passfruit>
         *      <accounts>
         *          <id-0000-0000-0000-0000>
         *              <provider>
         *                  provider name
         *              </provider>
         *              <fields>
         *                  <username>user name</username>
         *                  <email>info@example.org</email>
         *              </fields>
         *              <tags>
         *                  <tag-key />
         *              </tags>
         *          </id-0000-0000-0000-0000>
         *      </accounts>
         *      <passwords>
         *          <id-0000-0000-0000-0000>
         *              <default>
         *                  password
         *              </default>
         *          </id-0000-0000-0000-0000>
         *      </passwords>
         *  </passfruit>    
         */

        public XmlRepository(XmlRepositoryConfiguration configuration) : base(configuration) {
            LoadXDocument();
        }

        public new XmlRepositoryConfiguration Configuration {
            get { return (XmlRepositoryConfiguration) base.Configuration; }
        }

        private const string AccountIdPrefix = "id-";
        
        private const string TagPrefix = "tag-";

        private XDocument _xDoc;

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
            return GetPasswordElement(accountId, passwordKey).Value;
        }

        public override void SetPassword(Guid accountId, string password, string passwordKey = DefaultPasswordKey) {
            if (String.IsNullOrEmpty(passwordKey)) {
                passwordKey = DefaultPasswordKey;
            }
            GetPasswordElement(accountId, passwordKey).Value = password;
        }

        protected override IEnumerable<Guid> GetAllAccountIds() {
            return GetAccountIdsWithFilter(accountId => !AccountElementIsDeleted(accountId));
        }

        protected override IEnumerable<Guid> GetDeletedAccountIds() {
            return GetAccountIdsWithFilter(AccountElementIsDeleted);
        }
        private bool AccountElementIsDeleted(Guid accountId) {
            return GetAccountDeletedElement(accountId).Value == bool.TrueString;
        }

        private IEnumerable<Guid> GetAccountIdsWithFilter(Func<Guid, bool> filterAccount) {
            var accountsElement = GetAccountsElement();
            var accountIds = new List<Guid>();
            foreach (var element in accountsElement.Elements()) {
                var accountId = element.Name.LocalName.Remove(0, AccountIdPrefix.Length);
                Guid guid;
                if (Guid.TryParse(accountId, out guid) && filterAccount(guid)) {
                    accountIds.Add(guid);    
                }
            }
            return accountIds;
        }

        protected override IAccount LoadAccount(Guid accountId) {
            var accountFieldsElement = GetAccountFieldsElement(accountId);
            var account = Accounts.Create(GetProviderFieldElement(accountId).Value, accountId);
            foreach (var fieldElement in accountFieldsElement.Elements()) {
                var fieldTypeKey = (FieldTypeKey)Enum.Parse(typeof (FieldTypeKey), fieldElement.Name.LocalName, true);
                account.SetField(fieldTypeKey, fieldElement.Value);
            }
            account.SetClean();
            return account;
        }

        protected override void LoadAllFieldTypes() {
            // FieldTypes.Add(FieldTypes.Create());
        }

        private void LoadXDocument() {
            var xmlFile = new FileInfo(Configuration.XmlFilePath);
            
            if (xmlFile.Exists && xmlFile.Length > 0) {
                _xDoc = XDocument.Load(xmlFile.FullName);
            } else {
                _xDoc = new XDocument();
            }
        }

        private XElement GetPassfruitElement() {
            return GetOrCreateElement("passfruit", _xDoc); ;
        }

        private XElement GetPasswordsElement() {
            return GetOrCreateElement("passwords", GetPassfruitElement());
        }

        private XElement GetAccountPasswordsElement(Guid accountId) {
            return GetOrCreateElement(AccountIdPrefix + accountId, GetPasswordsElement());
        }

        private XElement GetPasswordElement(Guid accountId, string passwordKey) {
            return GetOrCreateElement(passwordKey, GetAccountPasswordsElement(accountId));
        }

        private XElement GetAccountsElement() {
            return GetOrCreateElement("accounts", GetPassfruitElement());
        }

        private XElement GetAccountElement(Guid accountId) {
            return GetOrCreateElement(AccountIdPrefix + accountId, GetAccountsElement());
        }

        private XElement GetAccountFieldsElement(Guid accountId) {
            return GetOrCreateElement("fields", GetAccountElement(accountId));
        }

        private XElement GetProviderFieldElement(Guid accountId) {
            return GetOrCreateElement("provider", GetAccountElement(accountId));
        }

        private XElement GetAccountDeletedElement(Guid accountId) {
            return GetOrCreateElement("deleted", GetAccountElement(accountId));
        }

        private XElement GetTagsElement() {
            return GetOrCreateElement("tags", GetPassfruitElement());
        }

        private XElement GetTagElement(string tagName) {
            return GetOrCreateElement(TagPrefix + tagName, GetPassfruitElement());
        }

        private XElement GetAccountFieldElement(IField field, Guid accountId) {
            return GetOrCreateElement(field.FieldType.Key.ToString(), GetAccountFieldsElement(accountId));
        }

        private XElement GetOrCreateElement(string elementName, XContainer parentElement) {
            elementName = elementName.ToLowerInvariant();
            var element = parentElement.Element(elementName);
            if (element == null) {
                element = new XElement(elementName);
                parentElement.Add(element);
            }
            return element;
        }

        public override void DeletePasswords(Guid accountId) {
            GetAccountPasswordsElement(accountId).Remove();
        }

        protected override void InternalSave(IAccount account) {
            GetAccountElement(account.Id).Remove();
            if (account.Provider != null) {
                GetProviderFieldElement(account.Id).Value = account.Provider.Key;
            }
            foreach (var field in account.Fields) {
                GetAccountFieldElement(field, account.Id).Value = field.Value.ToString();
            }
            foreach (var tag in account.Tags) {
                var tagElement = GetTagElement(tag.Key);
                // ToDo: Should handle adding, removing and changing tags
            }
            if (account is DeletedAccount) {
                GetAccountDeletedElement(account.Id).Value = bool.TrueString;
            }
            _xDoc.Save(Configuration.XmlFilePath);
        }

    }

}
