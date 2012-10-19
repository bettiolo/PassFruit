using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passfruit.Ui.ConsoleApp {

    public class OptionKeys {

        private Action _writeMessage;

        public OptionKeys(string message) {
            _writeMessage = message.WriteLine;
        }

        public OptionKeys(Action writeMessage) {
            _writeMessage = writeMessage;
        }

        private readonly IDictionary<string, Action> _keyActions = new Dictionary<string, Action>();

        private readonly IList<Action> _displayMessages = new List<Action>(); 

        public OptionKeys Option(string key, Action action) {
            _keyActions.Add(key, action);
            return this;
        }

        public OptionKeys Options(int numbers, Action<int> itemSelectedAction) {
            for (int i = 1; i <= numbers; i++) {
                var accountIndex = i - 1;
                Option(i.ToString(), () => itemSelectedAction(accountIndex));
            }
            return this;
        }

        public OptionKeys Options<T>(IEnumerable<T> items, Action<int, T> displayItem, Action<T> itemSelectedAction) {
            var index = 1;
            foreach (var item in items) {
                var localItem = item;
                var localIndex = index;
                _displayMessages.Add(() => displayItem(localIndex, localItem));
                Option(localIndex.ToString(), () => itemSelectedAction(localItem));
                index++;
            }
            return this;
        }

        public bool Choose(bool foreSelect = false) {
            return ConsoleChooser.Choose(_writeMessage, _keyActions, _displayMessages, foreSelect);
        }

        public void ChooseAndLoopOptions() {
            ConsoleChooser.ChooseAndLoopOptions(_writeMessage, _keyActions, _displayMessages);
        }

        public bool Confirm() {
            if (!_keyActions.Any() || _keyActions.Count > 1) {
                throw new NotSupportedException("The confirm dialog must have one and only one option");
            }
            return ConsoleChooser.Confirm(_writeMessage, _keyActions.First().Key, _keyActions.First().Value);
        }

        public OptionKeys WriteLine(string text)
        {
            _writeMessage = () =>
            {
                _writeMessage();
                text.WriteLine();
            };
            return this;
        }

        private class ConsoleChooser {

            public static void ChooseAndLoopOptions(Action message, IDictionary<string, Action> keyActions, IList<Action> displayMessages) {
                bool canceled;
                do {
                    canceled = !Choose(message, keyActions, displayMessages);
                } while (!canceled);
            }

            public static bool Choose(Action writeMessage, IDictionary<string, Action> keyActions, IList<Action> displayMessages, bool forceSelect = false) {
                var keyOptionsMessage = string.Format("(Options: {0}.{1}) ",
                    string.Join(", ", keyActions.Keys.Select(key => key.ToUpperInvariant())),
                    forceSelect ? "" : " Any other key cancels"
                );
                Action writeMessageAndKeys = () => {
                    writeMessage();
                    if (displayMessages.Any()) {
                        foreach (var displayMessage in displayMessages) {
                            displayMessage();
                        }
                        "".WriteLine();
                    }
                    keyOptionsMessage.Write();
                };
                var consoleChooser = new ConsoleChooser(writeMessageAndKeys);
                bool choosed;
                do {
                    choosed = consoleChooser.Choose(keyActions);
                    if (forceSelect && !choosed) {
                        "The selected option is invalid".Message();
                    }
                } while (forceSelect && !choosed);
                return choosed;
            }

            public static bool Confirm(Action writeMessage, string key, Action action) {
                var keyOptionsMessage = string.Format("('{0}' confirms. Any other key cancels) ", key.ToUpperInvariant());
                Action writeMessageAndKeys = () => {
                    writeMessage();
                    keyOptionsMessage.WriteLine();
                };
                var consoleChooser = new ConsoleChooser(writeMessageAndKeys);
                var confirmed = false;
                consoleChooser.Choose(new Dictionary<string, Action> {
                    { key, () => {
                        confirmed = true;
                        action();
                    }}
                });
                return confirmed;
            }

            private readonly Action _writeMessage;

            private ConsoleChooser(Action writeMessage) {
                _writeMessage = writeMessage;
            }

            public bool Choose(IDictionary<string, Action> keyActions) {
                _writeMessage();
                var userInput = Console.ReadLine();
                "".WriteLine();
                if (string.IsNullOrWhiteSpace(userInput)) {
                    return false;
                }
                foreach (var keyAction in keyActions) {
                    if (!userInput.Equals(keyAction.Key, StringComparison.OrdinalIgnoreCase)) {
                        continue;
                    }
                    keyAction.Value();
                    return true;
                }
                return false;
            }

        }

    }

}
