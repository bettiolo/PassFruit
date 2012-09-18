using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace Passfruit.Ui.ConsoleApp {

    internal class AccountDetailSection {

        private readonly AccountSection _parentSection;

        public AccountDetailSection(AccountSection parentSection) {
            _parentSection = parentSection;
        }

        public void EditAccount(IAccount account) {
            "Do you want to edit (T)ags, (F)ields, (N)otes or (D)elete the account?"
                .Option("t", () => EditTags(account))
                .Option("f", () => EditFields(account))
                .Option("n", () => EditNotes(account))
                .Option("d", () => _parentSection.DeleteAccount(account))
                .Choose();

            if (!account.IsDirty) {
                return;
            }

            var saved =
            "The account has been edited, press (S) to save it: "
                .Option("s", () => {
                    // ToDo: Save account
                    "Account saved.".Message();
                })
                .Confirm();

            if (saved) {
                return;
            }

            "Press (R) to revert the changes: "
                .Option("r", () => {
                    //ToDo: Revert account
                    "Account reverted.".Message();
                })
                .Choose();
        }

        private void EditTags(IAccount account) {
            // ToDo: Implement field editor
            "Not implemented.".Message();
        }

        private void EditFields(IAccount account) {
            // ToDo: Implement field editor
            "Not implemented.".Message();
        }

        private void EditNotes(IAccount account) {
            ("Note: " + account.Notes).Message();
            account.Notes = "Input new note: "
                .ReadLine();
            "Note saved.".Message();
        }

    }

}
