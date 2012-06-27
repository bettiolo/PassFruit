using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly InMemoryRepositoryConfiguration _configuration = new InMemoryRepositoryConfiguration(Path.GetTempFileName());

        protected override IRepository GetNewRepositoryWithFakeData() {
            var repository = new InMemoryRepository(_configuration);
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(repository);
            return repository;
        }

        protected override IRepository GetReloadedRepository() {
            return new InMemoryRepository(_configuration);
        }
    }

}
