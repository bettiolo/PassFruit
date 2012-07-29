using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using PassFruit.DataStore.Contracts;

namespace PassFruit.DataStore.XmlDataStore {

    public class XmlDataStore : DataStoreBase {

        private readonly XmlDataStoreConfiguration _configuration;

        /*  <passfruit>
         *      <accounts>
         *          <id-0000-0000-0000-0000 lastChanged="01/02/2003">
         *              ENCRYPTED START
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
         *              <note>
         *                  bla bla bla
         *              </note>
         *              ENCRYPTED END
         *          </id-0000-0000-0000-0000>
         *      </accounts>
         *      <passwords>
         *          <id-0000-0000-0000-0000 lastChangedUtc="01/02/2003">
         *              ENCRYPTED START
         *              <pwd-1111-1111-1111-1111 lastChangedUtc="01/02/2003">
         *                  <name>
         *                      bla bla bla
         *                  </name>
         *                  <password>
         *                      passwo0rd1
         *                  </password>
         *              <pwd-1111-1111-1111-1111>
         *              ENCRYPTED END
         *          </id-0000-0000-0000-0000>
         *      </passwords>
         *  </passfruit>    
         */

        public XmlDataStore(XmlDataStoreConfiguration configuration) {
            _configuration = configuration;
            LoadXDocument();
        }

        private const string AccountIdPrefix = "id-";
        private const string TagPrefix = "tag-";
        private const string PasswordIdPrefix = "pwd-";
        private const string FieldIdPrefix = "field-";

        private XDocument _xDoc;

        public override string Name {
            get { return "XML dataStore"; }
        }

        public override string Description {
            get { return "XML dataStore, the data is persisted in a XML file"; }
        }

        public override IEnumerable<Guid> GetAllAccountIds() {
            return GetAccountIdsWithFilter(accountId => !AccountElementIsDeleted(accountId));
        }

        public override IAccountDto GetAccountDto(Guid accountId) {
            return new AccountDto(accountId) {
                ProviderKey = GetProviderElement(accountId).Value,
                Fields = GetAccountFieldDtos(accountId),
                Tags = GetTags(accountId),
                Notes = GetNoteElement(accountId).Value,
                LastChangedUtc = GetLastChangedUtc(GetAccountElement(accountId))
            };
        }

        public override void SaveAccountDto(IAccountDto accountDto) {
            var originalAccountDto = GetAccountDto(accountDto.Id);
            if (originalAccountDto.Equals(accountDto)) {
                return;
            }
            GetAccountElement(accountDto.Id).Remove();
            GetProviderElement(accountDto.Id).Value = accountDto.ProviderKey;
            foreach (var field in accountDto.Fields) {
                GetFieldNameElement(accountDto.Id, field.Id).Value = field.Name;
                GetFieldTypeKeyElement(accountDto.Id, field.Id).Value = field.FieldTypeKey;
                GetFieldValueElement(accountDto.Id, field.Id).Value = field.Value.ToString();
            }
            foreach (var tag in accountDto.Tags) {
                GetTagElement(tag.Name, accountDto.Id);
            }
            GetNoteElement(accountDto.Id).Value = accountDto.Notes ?? "";
            var accountElement = GetAccountElement(accountDto.Id);
            UpdateLastChangedUtc(accountElement);
            SaveXml();
            accountDto.LastChangedUtc = GetLastChangedUtc(accountElement);
        }

        //public override void DeletedAccount(Guid accountId) {
        //    GetAccountElement(accountId).Remove();
        //    GetAccountDeletedElement(accountId).Value = bool.TrueString;
        //    SetLastChangedUtc(GetAccountElement(accountId));
        //    SaveXml();
        //}

        public override IEnumerable<IPasswordDto> GetPasswordDtos(Guid accountId) {
            foreach (var passwordElement in GetAccountPasswordsElement(accountId).Elements()) {
                if (!passwordElement.Name.LocalName.StartsWith(PasswordIdPrefix)) {
                    throw new Exception(string.Format("The password id '{0}' is not starting with the prefix '{1}'",
                        passwordElement.Name.LocalName, PasswordIdPrefix));
                }
                var passwordId = Guid.Parse(passwordElement.Name.LocalName.Remove(0, PasswordIdPrefix.Length));
                yield return new PasswordDto(passwordId) {
                    Name = GetPasswordNameElement(accountId, passwordId).Value,
                    Password = GetPasswordValueElement(accountId, passwordId).Value,
                    LastChangedUtc = GetLastChangedUtc(passwordElement)
                };
            }
        }

        public override void SavePasswordDto(IAccountDto accountDto, IPasswordDto passwordDto) {
            var accountId = accountDto.Id;
            var originalPasswordDto = GetPasswordDtos(accountId).SingleOrDefault(pwdDto => pwdDto.Id == accountId);
            if (originalPasswordDto != null && originalPasswordDto.Equals(passwordDto)) {
                return;
            }
            GetPasswordElement(accountId, passwordDto.Id).Remove();
            GetPasswordNameElement(accountId, passwordDto.Id).Value = passwordDto.Name;
            GetPasswordValueElement(accountId, passwordDto.Id).Value = passwordDto.Password;
            var accountElement = GetAccountElement(accountId);
            var passwordElement = GetPasswordElement(accountId, passwordDto.Id);
            UpdateLastChangedUtc(passwordElement);
            UpdateLastChangedUtc(GetAccountPasswordsElement(accountId));
            UpdateLastChangedUtc(accountElement);
            SaveXml();
            passwordDto.LastChangedUtc = GetLastChangedUtc(passwordElement);
            accountDto.LastChangedUtc = GetLastChangedUtc(accountElement);
        }

        public override void DeleteAccountPasswords(IAccountDto accountDto) {
            GetAccountPasswordsElement(accountDto.Id).Remove();
            var accountElement = GetAccountElement(accountDto.Id);
            UpdateLastChangedUtc(accountElement);
            SaveXml();
            accountDto.LastChangedUtc = GetLastChangedUtc(accountElement);
        }

        private IEnumerable<Guid> GetAccountIdsWithFilter(Func<Guid, bool> filterAccount) {
            var accountIds = new List<Guid>();
            GetElementsWithId(GetAccountsElement(), AccountIdPrefix, (accountId, _) => {
                if (filterAccount(accountId)) {
                    accountIds.Add(accountId);
                }
            });
            return accountIds;
        }

        private IList<ITagDto> GetTags(Guid accountId) {
            var tags = new List<ITagDto>();
            foreach (var tagElement in GetTagsElement(accountId).Elements()) {
                var tagElementName = tagElement.Name.LocalName;
                if (!tagElementName.StartsWith(TagPrefix)) {
                    throw new Exception(string.Format("The tag name '{0}' is not starting with the prefix '{1}'", 
                        tagElementName, TagPrefix));
                }
                var tagKey = tagElement.Name.LocalName.Remove(0, TagPrefix.Length);
                tags.Add(new TagDto { Name = tagKey });
            }
            return tags;
        }

        public IList<IFieldDto> GetAccountFieldDtos(Guid accountId) {
            var fieldDtos = new List<IFieldDto>();
            GetElementsWithId(GetAccountFieldsElement(accountId), FieldIdPrefix, (fieldId, _) =>
                fieldDtos.Add(new FieldDto(fieldId) {
                    FieldTypeKey = GetFieldTypeKeyElement(accountId, fieldId).Value,
                    Name = GetFieldNameElement(accountId, fieldId).Value,
                    Value = GetFieldValueElement(accountId, fieldId).Value
                }
            ));
            return fieldDtos;
        }

        private bool AccountElementIsDeleted(Guid accountId) {
            return GetAccountDeletedElement(accountId).Value == bool.TrueString;
        }

        private void UpdateLastChangedUtc(XElement element) {
            GetLastChangedUtcAttribute(element).Value = DateTime.UtcNow.Ticks.ToString();
        }

        private DateTime GetLastChangedUtc(XElement element) {
            var lastChangedUtcString = GetLastChangedUtcAttribute(element).Value;
            return string.IsNullOrWhiteSpace(lastChangedUtcString) 
                ? DateTime.MinValue 
                : new DateTime(long.Parse(lastChangedUtcString), DateTimeKind.Utc).ToUniversalTime();
        }

        private void LoadXDocument() {
            _xDoc = _configuration.GetXDoc();
        }

        private XElement GetPassfruitElement() {
            return GetOrCreateElement("passfruit", _xDoc);
        }

        private XElement GetPasswordsElement() {
            return GetOrCreateElement("passwords", GetPassfruitElement());
        }

        private XElement GetAccountPasswordsElement(Guid accountId) {
            return GetOrCreateElement(AccountIdPrefix + accountId, GetPasswordsElement());
        }

        private XElement GetPasswordElement(Guid accountId, Guid passwordId) {
            return GetOrCreateElement(PasswordIdPrefix + passwordId, GetAccountPasswordsElement(accountId));
        }

        private XElement GetPasswordNameElement(Guid accountId, Guid passwordId) {
            return GetOrCreateElement("name", GetPasswordElement(accountId, passwordId));
        }

        private XElement GetPasswordValueElement(Guid accountId, Guid passwordId) {
            return GetOrCreateElement("password", GetPasswordElement(accountId, passwordId));
        }

        private XAttribute GetLastChangedUtcAttribute(XElement element) {
            return GetOrCreateAttribute("lastChanged", element);
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

        private XElement GetFieldElement(Guid accountId, Guid fieldId) {
            return GetOrCreateElement(FieldIdPrefix + fieldId, GetAccountFieldsElement(accountId));
        }

        private XElement GetFieldNameElement(Guid accountId, Guid fieldId) {
            return GetOrCreateElement("name", GetFieldElement(accountId, fieldId));
        }

        private XElement GetFieldTypeKeyElement(Guid accountId, Guid fieldId) {
            return GetOrCreateElement("fieldType", GetFieldElement(accountId, fieldId));
        }

        private XElement GetFieldValueElement(Guid accountId, Guid fieldId) {
            return GetOrCreateElement("value", GetFieldElement(accountId, fieldId));
        }

        private XElement GetProviderElement(Guid accountId) {
            return GetOrCreateElement("provider", GetAccountElement(accountId));
        }
        
        private XElement GetAccountDeletedElement(Guid accountId) {
            return GetOrCreateElement("deleted", GetAccountElement(accountId));
        }

        private XElement GetTagsElement(Guid accountId) {
            return GetOrCreateElement("tags", GetAccountElement(accountId));
        }

        private XElement GetNoteElement(Guid accountId) {
            return GetOrCreateElement("note", GetAccountElement(accountId));
        }

        private XElement GetTagElement(string tagName, Guid accountId) {
            return GetOrCreateElement(TagPrefix + tagName, GetTagsElement(accountId));
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

        private XAttribute GetOrCreateAttribute(string attributeName, XElement element) {
            attributeName = attributeName.ToLowerInvariant();
            var attribute = element.Attribute(attributeName);
            if (attribute == null) {
                attribute = new XAttribute(attributeName, "");
                element.Add(attribute);
            }
            return attribute;
        }

        private void GetElementsWithId(XContainer containingElement, string idPrefix,
                               Action<Guid, XElement> processChildElement) {
            foreach (var childElement in containingElement.Elements()) {
                if (!childElement.Name.LocalName.StartsWith(idPrefix)) {
                    throw new Exception(string.Format("The element id '{0}' is not starting with the prefix '{1}'",
                        childElement.Name.LocalName, idPrefix));
                }
                var id = Guid.Parse(childElement.Name.LocalName.Remove(0, idPrefix.Length));
                processChildElement(id, childElement);
            }
        }

        private void SaveXml() {
            _configuration.SaveXdoc(_xDoc);
        }

    }

}
