using System;
using System.Collections;
using System.Collections.Generic;
using PassFruit.Contracts;

namespace PassFruit {

    public class FieldTypes : IFieldTypes {

        private readonly IRepository _repository;

        public FieldTypes(IRepository repository) {
            _repository = repository;
        }

        public IEnumerator<Contracts.FieldTypeName> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

    }

}