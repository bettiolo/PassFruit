
namespace PassFruit.DataStore.Contracts {

    public interface IProviderDto {

        string Key { get; set; }

        string Name { get; set; }

        bool HasEmail { get; set; }

        bool HasUserName { get; set; }

        bool HasPassword { get; set; }

        string Url { get; set; }

    }

}