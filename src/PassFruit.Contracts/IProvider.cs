namespace PassFruit.Contracts {

    public interface IProvider {

        string Key { get; }

        string Name { get; }

        bool HasEmail { get; }

        bool HasUserName { get; }

        bool HasPassword { get; }

        string Url { get; }
       
    }

}