namespace EnergyTrading.MDM.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RWEST.Nexus.MDM.Contracts;
    using EnergyTrading;
    using EnergyTrading.Data;
    using EnergyTrading.Mapping;
    using EnergyTrading.Search;
    using EnergyTrading.Validation;
    using RWEST.Nexus.MDM;
    using EnergyTrading.MDM.Messages;
    using EnergyTrading.MDM.Services;

    [TestClass]
    public class PartyCrossMapFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullRequestErrors()
        {
            // Arrange
            var validatorFactory = new Mock<IValidatorEngine>(); 
            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
            var searchCache = new Mock<ISearchCache>();

            var service = new PartyService(validatorFactory.Object, mappingEngine.Object, repository.Object, searchCache.Object);

            // Act
            service.CrossMap(null);
        }

        [TestMethod]
        public void UnsuccessfulMatchReturnsNotFound()
        {
            // Arrange
            var validatorFactory = new Mock<IValidatorEngine>(); 
            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
            var searchCache = new Mock<ISearchCache>();

            var service = new PartyService(validatorFactory.Object, mappingEngine.Object, repository.Object, searchCache.Object);

            var list = new List<PartyMapping>();
            repository.Setup(x => x.Queryable<PartyMapping>()).Returns(list.AsQueryable());

            var request = new CrossMappingRequest { SystemName = "Endur", Identifier = "A", ValidAt = SystemTime.UtcNow(), TargetSystemName = "Trayport" };

            // Act
            var contract = service.CrossMap(request);

            // Assert
            Assert.IsNotNull(contract, "Contract null");
            Assert.IsFalse(contract.IsValid, "Contract valid");
            Assert.AreEqual(ErrorType.NotFound, contract.Error.Type, "ErrorType difers"); ;
        }

        [TestMethod]
        public void SuccessMatch()
        {
            // Arrange
            var validatorFactory = new Mock<IValidatorEngine>(); 
            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
            var searchCache = new Mock<ISearchCache>();

            var service = new PartyService(validatorFactory.Object, mappingEngine.Object, repository.Object, searchCache.Object);

            // Domain details
            var system = new MDM.SourceSystem { Name = "Endur" };
            var mapping = new PartyMapping
                {
                    System = system,
                    MappingValue = "A",
                };
            var targetSystem = new MDM.SourceSystem { Name = "Trayport" };
            var targetMapping = new PartyMapping
                {
                    System = targetSystem,
                    MappingValue = "B",
                    IsDefault = true
                };
            var details = new MDM.PartyDetails
                {
                    Name = "Party 1"
                };
            var party = new MDM.Party
                {
                    Id = 1
                };
            party.AddDetails(details);
            party.ProcessMapping(mapping);
            party.ProcessMapping(targetMapping);

            // Contract details
            var targetIdentifier = new NexusId
                {
                    SystemName = "Trayport",
                    Identifier = "B"
                };

            mappingEngine.Setup(x => x.Map<PartyMapping, NexusId>(targetMapping)).Returns(targetIdentifier);

            var list = new List<PartyMapping> { mapping };
            repository.Setup(x => x.Queryable<PartyMapping>()).Returns(list.AsQueryable());

            var request = new CrossMappingRequest
            {
                SystemName = "Endur",
                Identifier = "A",
                TargetSystemName = "trayport",
                ValidAt = SystemTime.UtcNow(),
                Version = 1
            };

            // Act
            var response = service.CrossMap(request);
            var candidate = response.Contract;

            // Assert
            Assert.IsNotNull(response, "Contract null");
            Assert.IsNotNull(candidate, "Mapping null");
            Assert.AreEqual(1, candidate.Mappings.Count, "Identifier count incorrect");
            Assert.AreSame(targetIdentifier, candidate.Mappings[0], "Different identifier assigned");
        }

        [TestMethod]
        public void SuccessMatchSameVersion()
        {
            // Arrange
            var validatorFactory = new Mock<IValidatorEngine>();
            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
            var searchCache = new Mock<ISearchCache>();

            var service = new PartyService(validatorFactory.Object, mappingEngine.Object, repository.Object, searchCache.Object);

            // Domain details
            var system = new MDM.SourceSystem { Name = "Endur" };
            var mapping = new PartyMapping
            {
                System = system,
                MappingValue = "A",
            };
            var targetSystem = new MDM.SourceSystem { Name = "Trayport" };
            var targetMapping = new PartyMapping
            {
                System = targetSystem,
                MappingValue = "B",
                IsDefault = true
            };
            var details = new MDM.PartyDetails
            {
                Name = "Party 1"
            };
            var party = new MDM.Party
            {
                Id = 1
            };
            party.AddDetails(details);
            party.ProcessMapping(mapping);
            party.ProcessMapping(targetMapping);

            // Contract details
            var targetIdentifier = new NexusId
            {
                SystemName = "Trayport",
                Identifier = "B"
            };

            mappingEngine.Setup(x => x.Map<PartyMapping, NexusId>(targetMapping)).Returns(targetIdentifier);

            var list = new List<PartyMapping> { mapping };
            repository.Setup(x => x.Queryable<PartyMapping>()).Returns(list.AsQueryable());

            var request = new CrossMappingRequest
            {
                SystemName = "Endur",
                Identifier = "A",
                TargetSystemName = "trayport",
                ValidAt = SystemTime.UtcNow(),
                Version = 0
            };

            // Act
            var response = service.CrossMap(request);
            var candidate = response.Contract;

            // Assert
            Assert.IsNotNull(response, "Contract null");
            Assert.IsTrue(response.IsValid, "Contract invalid");
            Assert.IsNull(candidate, "Mapping not null");
        }
    }
}