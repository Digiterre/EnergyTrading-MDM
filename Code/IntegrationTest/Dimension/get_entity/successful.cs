namespace EnergyTrading.MDM.Test
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;

    using Microsoft.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class when_a_request_is_made_for_a_dimension_and_they_exist : IntegrationTestBase
    {
        private static MDM.Dimension dimension;

        private static RWEST.Nexus.MDM.Contracts.Dimension returnedDimension;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Establish_context();
            Because_of();
        }

        protected static void Establish_context()
        {
            dimension = DimensionData.CreateBasicEntity();
        }

        protected static void Because_of()
        {
            using (var client = new HttpClient(ServiceUrl["Dimension"] + 
                dimension.Id))
            {
                using (HttpResponseMessage response = client.Get())
                {
                    returnedDimension = response.Content.ReadAsDataContract<RWEST.Nexus.MDM.Contracts.Dimension>();
                }
            }
        }

        [TestMethod]
        public void should_return_the_dimension_with_the_correct_details()
        {
            DimensionDataChecker.CompareContractWithSavedEntity(returnedDimension);
        }
    }

    [TestClass]
    public class when_a_request_is_made_for_a_dimension_as_of_a_date_and_they_exist : IntegrationTestBase
    {
        private static MDM.Dimension dimension;
        private static RWEST.Nexus.MDM.Contracts.Dimension returnedDimension;
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
            dimension = DimensionData.CreateBasicEntity();
        }

        protected static void Because_of()
        {
            asof = Script.baseDate.AddSeconds(1);
            client =
                new HttpClient(ServiceUrl["Dimension"] + string.Format("{0}?as-of={1}",
                    dimension.Id.ToString(), asof.ToString(DateFormatString)));

            HttpResponseMessage response = client.Get();
            returnedDimension = response.Content.ReadAsDataContract<RWEST.Nexus.MDM.Contracts.Dimension>();
        }

        [TestMethod]
        public void should_return_the_dimension_with_the_correct_details()
        {
            DimensionDataChecker.CompareContractWithSavedEntity(returnedDimension);
        }
    }
}