using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PassFruit.Contracts;
using PassFruit.Tests;
using PassFruit.Tests.FakeData;

namespace PassFruit.Client.XmlRepository.Tests {

    [TestFixture]
    public class XmlRepositoryTests : RepositoryTests {

        private XmlRepositoryConfiguration _configuration;

        protected override IRepository GetNewRepositoryWithFakeData() {
            if (File.Exists(_configuration.XmlFilePath)) {
                File.Delete(_configuration.XmlFilePath);
            }
            _configuration = new XmlRepositoryConfiguration(Path.GetTempFileName());
            var repository = new XmlRepository(_configuration);
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(repository);
            return repository;
        }

        protected override IRepository GetReloadedRepository() {
            return new XmlRepository(_configuration);               
        }

    }

}
