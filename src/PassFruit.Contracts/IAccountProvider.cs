namespace PassFruit.Contracts {

    public interface IAccountProvider {

        string Name { get; }

        bool HasEmail { get; }

        bool HasUserName { get; }

        string Url { get; }
       
    }

}