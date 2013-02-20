using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PassFruit.Server.FakeDataGenerator;

namespace PassFruit.Server.CiphertextDatastore.Tests
{
    public class MockedCiphertextDatastoreTest : CiphertextDatastoreTestBase
    {
        protected override CiphertextDatastoreBase CreateEmpty()
        {
            var mockedCiphertextDatastore = new Mock<CiphertextDatastoreBase>(MockBehavior.Strict, Guid.NewGuid()) { CallBase = true };
            var cipheredAccountDtos = new Dictionary<Guid, CipheredAccountDto>();
            mockedCiphertextDatastore
                .Setup(cds => cds.InternalSave(It.Is<CipheredAccountDto>(
                    cipheredAccountDto => cipheredAccountDtos[cipheredAccountDto.Id] = cipheredAccountDto)));
            return mockedCiphertextDatastore.Object;
        }

        protected override CiphertextDatastoreBase CreatePopulatedWithFakeData()
        {
            var mockedCiphertextDatastore = CreateEmpty();
            CipheredAccountFakeDataGenerator.Populate(mockedCiphertextDatastore);
            return mockedCiphertextDatastore;
        }

        protected override CiphertextDatastoreBase CreateReloadedPopulatedWithFakeData()
        {
            var mockedCiphertextDatastore = CreatePopulatedWithFakeData();
            return mockedCiphertextDatastore;
        }
    }
}
