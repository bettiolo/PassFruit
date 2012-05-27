using PassFruit.Contracts;

namespace PassFruit {

    public class Provider : IProvider {

        internal Provider(string key) {
            Key = key;
        }

        public string Key { get; private set; }

        public string Name { get; internal set; }

        public bool HasEmail { get; internal set; }

        public bool HasUserName { get; internal set; }

        public bool HasPassword { get; internal set; }

        public string Url { get; internal set; }

        public override string ToString() {
            return Name;
        }

    }

}
