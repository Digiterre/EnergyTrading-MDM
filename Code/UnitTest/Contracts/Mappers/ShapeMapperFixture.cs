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
    public class ShapeMapperFixture : Fixture
    {
        [TestMethod]
        public void Resolve()
        {
            var container = CreateContainer();
            var meConfig = new SimpleMappingEngineConfiguration(container);
            meConfig.Configure();

            var config = new ShapeConfiguration(container);
            config.Configure();

            var validator = container.Resolve<IMapper<RWEST.Nexus.MDM.Contracts.Shape, Shape>>();

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
            var contractDetails = new RWEST.Nexus.MDM.Contracts.ShapeDetails();
            var contract = new RWEST.Nexus.MDM.Contracts.Shape
            {
                Identifiers = new RWEST.Nexus.MDM.Contracts.NexusIdList { id },
                Details = contractDetails,
                Nexus = new RWEST.Nexus.MDM.Contracts.SystemData { StartDate = start, EndDate = end }
            };

            // NB Don't assign validity here, want to prove SUT sets it
            var details = new Shape();

            var mapping = new ShapeMapping();

            var mappingEngine = new Mock<IMappingEngine>();
            mappingEngine.Setup(x => x.Map<RWEST.Nexus.MDM.Contracts.NexusId, ShapeMapping>(id)).Returns(mapping);
            mappingEngine.Setup(x => x.Map<RWEST.Nexus.MDM.Contracts.ShapeDetails, Shape>(contractDetails)).Returns(details);

            var mapper = new ShapeMapper(mappingEngine.Object);

            // Act
            var candidate = mapper.Map(contract);

            // Assert
            //Assert.AreEqual(1, candidate.Details.Count, "Detail count differs");
            Assert.AreEqual(1, candidate.Mappings.Count, "Mapping count differs");
            Check(range, details.Validity, "Validity differs");
        }
    }
}
