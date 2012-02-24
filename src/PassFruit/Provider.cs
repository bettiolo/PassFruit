using PassFruit.Contracts;

namespace PassFruit {

    public class Provider : IProvider {

        internal Provider() {
            
        }

        public string Name { get; private set; }

        public bool HasEmail { get; private set; }

        public bool HasUserName { get; private set; }

        public bool HasPassword { get; private set; }

        public string Url { get; private set; }

    }

}
