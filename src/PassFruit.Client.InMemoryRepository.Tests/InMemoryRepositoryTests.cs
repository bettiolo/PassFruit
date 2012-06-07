using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Contracts;
using PassFruit.Tests;
using PassFruit.Tests.FakeData;

namespace PassFruit.Client.InMemoryRepository.Tests {

    [TestFixture]
    public class InMemoryRepositoryTests : RepositoryTests {

        protected override IRepository GetRepositoryWithFakeData() {
            var repository = new InMemoryRepository();
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(repository);
            return repository;
        }

    }

}
