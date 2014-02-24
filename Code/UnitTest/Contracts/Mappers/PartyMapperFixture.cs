namespace EnergyTrading.MDM.Test.Contracts.Mappers
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.MDM.Configuration;
    using EnergyTrading.MDM.Contracts.Mappers;
    using EnergyTrading;
    using EnergyTrading.Mapping;

    using RWEST.Nexus.MDM;

    [TestClass]
    public class PartyMapperFixture : Fixture
    {
        [TestMethod]
        public void Resolve()
        {
            var container = CreateContainer();
            var meConfig = new SimpleMappingEngineConfiguration(container);
            meConfig.Configure();

            var config = new PartyConfiguration(container);
            config.Configure();

            var validator = container.Resolve<IMapper<RWEST.Nexus.MDM.Contracts.Party, Party>>();

            // Assert
            Assert.IsNotNull(validator, "Mapper resolution failed");
        }

        [TestMethod]
        public void Map()
        {
            // Arrange
            var start = new DateTime(2010, 1, 1);
            var end = DateUtility.MaxDate;
            var range = new DateRange(start, end);

            var id = new RWEST.Nexus.MDM.Contracts.NexusId { SystemName = "Test", Identifier = "A" };
            var contractDetails = new RWEST.Nexus.MDM.Contracts.PartyDetails();
            var contract = new RWEST.Nexus.MDM.Contracts.Party
                {
                    Identifiers = new RWEST.Nexus.MDM.Contracts.NexusIdList { id },
                    Details = contractDetails,
                    Nexus = new RWEST.Nexus.MDM.Contracts.SystemData { StartDate = start, EndDate = end }
                };

            // NB Don't assign validity here, want to prove SUT sets it
            var partyDetails = new PartyDetails();

            var mapping = new PartyMapping();

            var mappingEngine = new Mock<IMappingEngine>();
            mappingEngine.Setup(x => x.Map<RWEST.Nexus.MDM.Contracts.NexusId, PartyMapping>(id)).Returns(mapping);
            mappingEngine.Setup(x => x.Map<RWEST.Nexus.MDM.Contracts.PartyDetails, PartyDetails>(contractDetails)).Returns(partyDetails);

            var mapper = new PartyMapper(mappingEngine.Object);

            // Act
            var candidate = mapper.Map(contract);

            // Assert
            Assert.AreEqual(1, candidate.Details.Count, "Detail count differs");
            Assert.AreEqual(1, candidate.Mappings.Count, "Mapping count differs");
            Check(range, partyDetails.Validity, "Validity differs");
        }
    }
}