using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassFruit.Server.CiphertextDatastore;

namespace PassFruit.Server.FakeDataGenerator
{
    public static class CipheredAccountFakeDataGenerator
    {

        public static CipheredAccountDto GetFakeCipheredAccountDto1()
        {
            return new CipheredAccountDto()
            {
                Ciphertext = Convert.FromBase64String("Buwwn66Vz+jXt2u9R3ZGSuuPpKYltm68X2uN0A0Lccs="),
                InitializationVector = Convert.FromBase64String("JAlU5q+H9f5rOzfmasNB/g=="),
                Salt = Convert.FromBase64String("dlKox+7Fws/uo2S0YJ6S6goxXnZ4HHZS")
            };
        }

        public static CipheredAccountDto GetFakeCipheredAccountDto2()
        {
            return new CipheredAccountDto()
            {
                Ciphertext = Convert.FromBase64String("HG0rRwghSEZurf76JVuv6nxHNd9eqUnFnVhH023hhnE="),
                InitializationVector = Convert.FromBase64String("3ey3JHzgm0XOKEq35YpkcQ=="),
                Salt = Convert.FromBase64String("Bepf0201JDbKDowZ4ZvG/fc/tJgp3559")
            };
        }

        public static CipheredAccountDto[] PopulatedAccounts
        {
            get
            {
                return new[]
                {
                    GetFakeCipheredAccountDto1(),
                    GetFakeCipheredAccountDto2()
                };
            }
        } 

        public static CipheredAccountDto GetFakeCipheredAccountDtoNotYetAdded()
        {
            return new CipheredAccountDto()
            {
                Ciphertext = Convert.FromBase64String("298df3UbUUXMcmMlY825NiZ1f2U3Y9hS/Zp09vxa3MM="),
                InitializationVector = Convert.FromBase64String("zihXBM3HaNqR27w1LLXEvw=="),
                Salt = Convert.FromBase64String("+xYXIJdogjQNsU6U+3wmJiUkX2fZ6qyx")
            };
        }


        public static void Populate(CiphertextDatastoreBase jsonCipheredDatastore)
        {
            jsonCipheredDatastore.Save(GetFakeCipheredAccountDto1());
            jsonCipheredDatastore.Save(GetFakeCipheredAccountDto2());
        }
    }
}
