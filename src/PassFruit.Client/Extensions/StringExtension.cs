using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System {

    public static class StringExtension {

        public static TEnum ToEnum<TEnum>(this string enumValue) where TEnum : struct {
            if (String.IsNullOrWhiteSpace(enumValue)) {
                return default(TEnum);
            }
            return (TEnum) Enum.Parse(typeof (TEnum), enumValue, true);
        }

    }

}
