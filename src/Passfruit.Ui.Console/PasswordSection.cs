using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit;
using PassFruit.Contracts;

namespace PassFruit.Ui.ConsoleApp {

    internal class PasswordSection {

        public void ChoosePassword(IAccount account) {
            var accountPasswords = account.GetPasswords();
            "Account's passwords:"
                .Options(accountPasswords,
                        (index, password) => string.Format("  - ({0}) {1}", index, password).WriteLine(),
                        ShowPassword)
                .WriteLine("Insert the number of the password to display")
                .ChooseAndLoopOptions();
        }

        private void ShowPassword(IPassword password) {
            (" - Name: " + password.Name).WriteLine();
            (" - Type: " + password.PasswordType.Description).WriteLine();
        }

    }

}
