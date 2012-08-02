using System;

namespace PassFruit {
    public interface IPassword {
        string Name { get; set; }

        string Value { get; set; }

        Guid Id { get; }
    }
}