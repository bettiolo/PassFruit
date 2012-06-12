using System;
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

        protected override IRepository GetRepositoryWithFakeData() {
            var repository = new XmlRepository(Path.GetTempFileName());
            var fakeDataGenerator = new FakeDataGenerator();
            fakeDataGenerator.GenerateFakeData(repository);
            return repository;
        }

        [Test]
        public void When_A_Field_Or_Password_Is_Changed_Then_The_Data_Should_Be_Persisted_After_Reload() {

            // Given
            var repository = (XmlRepository)GetRepositoryWithFakeData();
            XmlRepository reloadedRepository = null;
            var account = repository.Accounts.GetByUserName("tWiTTeRUsEr").First();
            IAccount reloadedAccount = null;
            var originalDefaultPassword = account.GetPassword();
            var originalCustomPassword = account.GetPassword("custom");
            var newDefaultPassword = "new password 123131 \"\"\"\"$£&!£&!\'\'\'\'|!£!><><>><";
            var newCustomPassword = "new Pasw0rd";
            var newEmail = "test@example.org";

            // When
            var actChangeDefaultPassword = new Action(() => account.SetPassword(newDefaultPassword));
            var actChangeCustomPassword = new Action(() => account.SetPassword(newCustomPassword, "CuStoM"));
            var actChangeEmail = new Action(() => account.SetField(FieldTypeKey.Email, newEmail));
            var actSave = new Action(account.Save);
            var actReloadRepository = new Action(() => reloadedRepository = new XmlRepository(repository.XmlFilePath));
            var actReloadAccount = new Action(() =>
                reloadedAccount = reloadedRepository.Accounts.GetByUserName("tWiTTeRUsEr").First()
            );

            // Then
            account.IsDirty.Should().BeFalse();
            account.GetPassword().Should().Be(originalDefaultPassword);
            account.GetPassword("CUSToM").Should().Be(originalCustomPassword);
            actChangeDefaultPassword();
            actChangeCustomPassword();
            account.IsDirty.Should().BeFalse(); // If a password is changed the dirty flag should not be set.
            actChangeEmail();
            account.IsDirty.Should().BeTrue();
            actSave();
            account.IsDirty.Should().BeFalse();
            account.GetPassword().Should().Be(newDefaultPassword);
            account.GetPassword("CUStom").Should().Be(newCustomPassword);
            account.GetFieldsByKey(FieldTypeKey.Email).First().Value.Should().Be(newEmail);
            actReloadRepository();
            reloadedRepository.Should().NotBeNull();
            actReloadAccount();
            reloadedAccount.Should().NotBeNull();
            reloadedAccount.IsDirty.Should().BeFalse();
            reloadedAccount.GetPassword().Should().Be(newDefaultPassword);
            reloadedAccount.GetPassword("CUStom").Should().Be(newCustomPassword);
            reloadedAccount.GetFieldsByKey(FieldTypeKey.Email).First().Value.Should().Be(newEmail);

        }

    }

}
