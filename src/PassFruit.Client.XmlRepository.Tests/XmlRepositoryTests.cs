using System;
using System.IO;
using NUnit.Framework;
using PassFruit.Contracts;
using PassFruit.Tests;
using PassFruit.Tests.FakeData;

namespace PassFruit.Client.XmlRepository.Tests {

    [TestFixture]
    public class XmlRepositoryTests : RepositoryTests {

        protected override IRepository GetRepositoryWithFakeData() {
            var repository = new XmlRepository(Path.GetTempFileName());
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(repository);
            return repository;
        }

    }

}
