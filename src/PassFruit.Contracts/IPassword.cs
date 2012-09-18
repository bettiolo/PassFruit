using System;
using PassFruit.Contracts;

namespace PassFruit {
    public interface IPassword {
        string Name { get; set; }

        string Value { get; set; }

        Guid Id { get; }

        IPasswordType PasswordType { get; }
    }
}