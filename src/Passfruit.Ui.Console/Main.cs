using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PassFruit;
using PassFruit.Contracts;
using PassFruit.DataStore;
using PassFruit.DataStore.Tests.FakeData;
using PassFruit.DataStore.XmlDataStore;

namespace Passfruit.Ui.ConsoleApp {
    
    public class Main {

        private readonly Accounts _accounts;

        private readonly XmlDataStore _dataStore;

        public Main() {
            const string xmlFilePath = @"C:\passfruit.xml";
            var fileExists = File.Exists(xmlFilePath);

            var xmlDataStoreConfiguration = new XmlDataStoreConfiguration(
                () => fileExists ? XDocument.Load(xmlFilePath) : new XDocument(),
                xDocument => xDocument.Save(xmlFilePath)
            );
            _dataStore = new XmlDataStore(xmlDataStoreConfiguration);

            if (!fileExists) {
                var fakeDataGenerator = new FakeDataGenerator();
                fakeDataGenerator.GenerateFakeData(_dataStore);
            }

            _accounts = new Accounts(_dataStore);
        }

        public void Run() {
            Console.WriteLine("Passfruit");
            Console.WriteLine();

            do {
                WriteAccountList();
            } while (WriteAccountSelector());

            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
        }

        public void WriteAccountList() {
            Console.WriteLine(string.Format("Accounts ({0}):", _accounts.Count()));
            var i = 1;
            foreach (var account in _accounts) {
                var edited = "[edited] ";
                Console.WriteLine(string.Format(" {0}) {2}{1}", i, account.GetAccountName(), 
                                                                account.IsDirty ? edited : ""));
                i++;
            }
            Console.WriteLine();
        }

        private bool WriteAccountSelector() {
            Console.Write("Insert the number of the account to show or any other key to exit: ");
            var accountNumberString = Console.ReadLine();
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(accountNumberString)) {
                var accountNumber = 0;
                if (!int.TryParse(accountNumberString, out accountNumber) 
                    || accountNumber > _accounts.Count()) {
                    return false;
                }
                var account = _accounts.Skip(accountNumber - 1).First();
                DisplayAccount(account);
                Console.WriteLine();
                return true;
            }
            return false;
        }

        private void DisplayAccount(IAccount account) {
            Console.WriteLine("Account: " + account);
            // Console.WriteLine(" - ID: " + account.Id);
            Console.WriteLine(" - Last Changed: " + account.LastChangedUtc);
            Console.WriteLine(" - Tags: " + account.Tags);
            Console.WriteLine(" - Fields: ");
            var i = 1;
            foreach (var field in account.Fields) {
                Console.WriteLine("  - Field " + i);
                DisplayField(field);
                i++;
            }
            Console.WriteLine(" - Notes: " + account.Notes);
            Console.WriteLine();
            EditAccount(account);
        }

        private void DisplayField(IField field) {
            // Console.WriteLine("   - ID: " + field.Id);
            Console.WriteLine("   - Type: " + field.FieldType);
            Console.WriteLine("   - Name: " + field.Name);
            Console.WriteLine("   - Value: " + field.Value);
        }


        private void EditAccount(IAccount account) {
            bool keyMatched;
            do {
                keyMatched = false;
                Console.Write("Do you want to edit (T)ags, (F)ields, (N)otes or (D)elete the account? Any other key to go back. ");
                var userInput = Console.ReadLine();
                Console.WriteLine();
                if (userInput.Equals("t", StringComparison.OrdinalIgnoreCase)) {
                    EditTags(account);
                    keyMatched = true;
                }
                if (userInput.Equals("f", StringComparison.OrdinalIgnoreCase)) {
                    EditFields(account);
                    keyMatched = true;
                }
                if (userInput.Equals("n", StringComparison.OrdinalIgnoreCase)) {
                    EditNotes(account);
                    keyMatched = true;
                }
                if (userInput.Equals("d", StringComparison.OrdinalIgnoreCase)) {
                    if (DeleteAccount(account)) {
                        return;
                    };
                    keyMatched = true;
                }
            } while (keyMatched);
            
            if (!account.IsDirty) {
                return;
            }
            
            Console.Write("The account has been edited, press S to save it: ");
            var saveAccount = Console.ReadLine().Equals("S", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine();
            if (saveAccount) {
                // ToDo: Save account
                Console.WriteLine("Account saved.");
                Console.WriteLine();
                return;
            }

            Console.WriteLine("Press R to revert the changes: ");
            var revertAccount = Console.ReadLine().Equals("r", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine();
            if (revertAccount) {
                //ToDo: Revert account
                Console.WriteLine("Account reverted.");
                Console.WriteLine();
                return;
            }

        }

        private void EditTags(IAccount account) {
            // ToDo: Implement field editor
            Console.WriteLine("Not implemented.");
            Console.WriteLine();
        }

        private void EditFields(IAccount account) {
            // ToDo: Implement field editor
            Console.WriteLine("Not implemented.");
            Console.WriteLine();
        }

        private void EditNotes(IAccount account) {
            Console.WriteLine("Note: " + account.Notes);
            Console.WriteLine();
            Console.WriteLine("Input new note:");
            var newNote = Console.ReadLine();
            account.Notes = newNote;
            Console.WriteLine();
            Console.WriteLine("Note saved.");
            Console.WriteLine();
        }

        private bool DeleteAccount(IAccount account) {
            Console.Write("Are you sure? Press Y to confirm: ");
            if (Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase)) {
                Console.ReadLine();
                // ToDo: Implement account deleting
                Console.Write("Not implemented");
                Console.ReadLine();
                return true;
            }
            Console.ReadLine();
            return false;
        }

    }

}
