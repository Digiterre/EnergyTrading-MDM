namespace EnergyTrading.MDM.Test
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;

    using Microsoft.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class when_a_request_is_made_for_a_location_and_they_exist : IntegrationTestBase
    {
        private static MDM.Location location;

        private static RWEST.Nexus.MDM.Contracts.Location returnedLocation;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Establish_context();
            Because_of();
        }

        protected static void Establish_context()
        {
            location = Script.LocationData.CreateBasicEntity();
        }

        protected static void Because_of()
        {
            using (var client = new HttpClient(ServiceUrl["Location"] + 
                location.Id))
            {
                using (HttpResponseMessage response = client.Get())
                {
                    returnedLocation = response.Content.ReadAsDataContract<RWEST.Nexus.MDM.Contracts.Location>();
                }
            }
        }

        [TestMethod]
        public void should_return_the_location_with_the_correct_details()
        {
            Script.LocationDataChecker.CompareContractWithSavedEntity(returnedLocation);
        }
    }

    [TestClass]
    public class when_a_request_is_made_for_a_location_as_of_a_date_and_they_exist : IntegrationTestBase
    {
        private static MDM.Location location;
        private static RWEST.Nexus.MDM.Contracts.Location returnedLocation;
        private static DateTime asof;
        private static HttpClient client;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Establish_context();
            Because_of();
        }

        protected static void Establish_context()
        {
            location = Script.LocationData.CreateBasicEntity();
        }

        protected static void Because_of()
        {
            asof = Script.baseDate.AddSeconds(1);
            client =
                new HttpClient(ServiceUrl["Location"] + string.Format("{0}?as-of={1}",
                    location.Id.ToString(), asof.ToString(DateFormatString)));

            HttpResponseMessage response = client.Get();
            returnedLocation = response.Content.ReadAsDataContract<RWEST.Nexus.MDM.Contracts.Location>();
        }

        [TestMethod]
        public void should_return_the_location_with_the_correct_details()
        {
            Script.LocationDataChecker.CompareContractWithSavedEntity(returnedLocation);
        }
    }
}