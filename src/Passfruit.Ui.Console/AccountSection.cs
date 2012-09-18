using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit;
using PassFruit.Contracts;
using PassFruit.DataStore.Contracts;

namespace Passfruit.Ui.ConsoleApp {

    internal class AccountSection {

        private readonly Accounts _accounts;

        public AccountSection(IDataStore dataStore) {
            _accounts = new Accounts(dataStore);
        }

        public void ChooseAccount() {
            var optionKeys = new OptionKeys(() => {
                WriteAccountList();
                "Insert the number of the account to display:".WriteLine();
            });
            optionKeys.Options(_accounts.Count(),
                accountIndex => {
                    var account = _accounts.Skip(accountIndex).First();
                    DisplayAccount(account);
                })
                .ChooseAndLoopOptions();
        }

        private void WriteAccountList() {
            "Accounts:".WriteLine();
            var i = 1;
            foreach (var account in _accounts) {
                var edited = "[edited] ";
                string.Format(" ({0}) {2}{1}", i, account, account.IsDirty ? edited : "").WriteLine();
                i++;
            }
            "".WriteLine();
        }

        private void DisplayAccount(IAccount account) {
            ("Account: " + account).WriteLine();
            // (" - ID: " + account.Id).WriteLine();
            (" - Provider: " + account.Provider).WriteLine();
            (" - Tags: " + account.Tags).WriteLine();
            " - Fields: ".WriteLine();
            var i = 1;
            foreach (var field in account.Fields) {
                ("  - Field " + i).WriteLine();
                // ("   - ID: " + field.Id).Message();
                ("   - Type: " + field.FieldType).WriteLine();
                ("   - Name: " + field.Name).WriteLine();
                ("   - Value: " + field.Value).WriteLine();
                i++;
            }
            (" - Notes: " + account.Notes).WriteLine();
            (" - Last Changed: " + account.LastChangedUtc).Message();
            ShowPasswordsOrEditAccount(account);
        }

        private void ShowPasswordsOrEditAccount(IAccount account) {
            var passwordSection = new PasswordSection();
            var accountDetailSection = new AccountDetailSection(this);
            "Do you want to view (P)asswords or (E)dit account?"
                .Option("p", () => passwordSection.ChoosePassword(account))
                .Option("e", () => accountDetailSection.EditAccount(account))
                .Choose();
        }

        internal bool DeleteAccount(IAccount account) {
            var delete =
            "Are you sure? Press (Y) to confirm: "
                .Option("y", () => {
                    // ToDo: Implement account deleting
                    "Not implemented".Message();
                })
                .Confirm();
            return delete;
        }




    }

}
