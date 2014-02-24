namespace EnergyTrading.MDM.Test.Mappers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
	using EnergyTrading.MDM.Test;

    [TestClass]
    public class PortfolioDetailsMapperFixture : Fixture
    {
	    [TestMethod]
        public void Map()
        {
            // Arrange
	        var source = ObjectMother.Create<Portfolio>();

            var mapper = new MDM.Mappers.PortfolioDetailsMapper();

            // Act
            var candidate = mapper.Map(source);

            // Assert
            Assert.AreEqual(source.Name, candidate.Name);
            Assert.AreEqual(source.PortfolioType, candidate.PortfolioType);
		}
    }
}

	