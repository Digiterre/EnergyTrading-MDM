namespace EnergyTrading.MDM.Test.Data.EF
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RWEST.Nexus.MDM;

    [TestClass]
    public class CommodityFeeTypeMappingRepositoryFixture : DbSetRepositoryFixture<CommodityFeeTypeMapping>
    {
        protected override CommodityFeeTypeMapping Default()
        {
            var entity = base.Default();
            entity.CommodityFeeType = ObjectMother.Create<CommodityFeeType>();
            entity.System = new SourceSystem { Name = Guid.NewGuid().ToString() };
            entity.MappingValue = "Test";

            return entity;
        }
    }
}
