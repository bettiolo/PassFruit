namespace PassFruit.Contracts {

    public interface IProvider {

        string Name { get; }

        bool HasEmail { get; }

        bool HasUserName { get; }

        bool HasPassword { get; }

        string Url { get; }
       
    }

}