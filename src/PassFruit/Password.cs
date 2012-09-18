using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;
using PassFruit.DataStore.Contracts;

namespace PassFruit {

    public class Password : IPassword {

        public Password (IPasswordDto passwordDto) {
            Id = passwordDto.Id;
            Name = passwordDto.Name;
            Value = passwordDto.Password;
            PasswordType = new PasswordType(passwordDto.PasswordTypeKey.ToEnum<PasswordTypeKey>());
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public IPasswordType PasswordType { get; private set; }

        public override string ToString() {
            return Name;
        }

    }
}
