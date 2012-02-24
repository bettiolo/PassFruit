﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {

    public interface IFieldTypes : IEnumerable<IFieldType> {

        IField<TValue> CreateField<TValue>(FieldTypeKey key, TValue value);

    }

}
