namespace EnergyTrading.MDM.Test
{
    using System;
    using System.Net;

    using EnergyTrading.MDM.Extensions;

    using Microsoft.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RWEST.Nexus.MDM.Contracts;
    using EnergyTrading.Data.EntityFramework;
    using EnergyTrading.MDM.Data.EF.Configuration;

    [TestClass]
    public class when_a_request_is_made_to_update_a_partyaccountability_mapping_and_the_mapping_doesnt_exist : IntegrationTestBase
    {
        private static HttpResponseMessage response;

        private static Mapping mapping;

        private static HttpContent content;

        private static HttpClient client;

        private static PartyAccountabilityMapping currentTrayportMapping;

        private static MDM.PartyAccountability entity;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Establish_context();
            Because_of();
        }

        protected static void Establish_context()
        {
            entity = PartyAccountabilityData.CreateBasicEntityWithOneMapping();
            currentTrayportMapping = entity.Mappings[0];

            mapping = new Mapping {
                
                    SystemName = currentTrayportMapping.System.Name,
                    Identifier = currentTrayportMapping.MappingValue,
                    SourceSystemOriginated = currentTrayportMapping.IsMaster,
                    DefaultReverseInd = currentTrayportMapping.IsDefault,
                    StartDate = currentTrayportMapping.Validity.Start,
                    EndDate = currentTrayportMapping.Validity.Finish.AddDays(2)
                };

            content = HttpContentExtensions.CreateDataContract(mapping);
            client = new HttpClient();
        }

        protected static void Because_of()
        {
            client.DefaultHeaders.Add("If-Match", CurrentEntityVersion().ToString());
            response = client.Post(ServiceUrl["PartyAccountability"] +  string.Format("{0}/Mapping/{1}", entity.Id, int.MaxValue), content);
        }

        [TestMethod]
        public void should_return_a_not_found_status_code()
        {
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        private static ulong CurrentEntityVersion()
        {
            var partyaccountabilityMapping = new DbSetRepository<MDM.PartyAccountabilityMapping>(new MappingContext()).FindOne(entity.Mappings[0].Id);
            return partyaccountabilityMapping.Version.ToUnsignedLongVersion();
        }
    }
}

