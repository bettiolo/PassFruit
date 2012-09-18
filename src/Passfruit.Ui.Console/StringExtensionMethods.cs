
using System.Collections.Generic;
using System.Linq;
using Passfruit.Ui.ConsoleApp;

namespace System {

    public static class StringExtensionMethods {

        public static void Message(this string message) {
            Console.WriteLine(message);
            Console.WriteLine();
        }

        public static void WriteLine(this string message) {
            Console.WriteLine(message);
        }

        public static void Write(this string message) {
            Console.Write(message);
        }

        public static string ReadLine(this string message) {
            Console.Write(message);
            var userInput = Console.ReadLine();
            Console.WriteLine();
            return userInput;
        }

        public static OptionKeys Option(this string message, string key, Action action) {
            return new OptionKeys(message).Option(key, action);
        }

        public static OptionKeys Options<T>(this string message, IEnumerable<T> items, Action<int, T> displayItem, Action<T> itemSelectedAction) {
            var optionKeys = new OptionKeys(message);
            return optionKeys.Options(items, displayItem, itemSelectedAction);
        }

    }

}
