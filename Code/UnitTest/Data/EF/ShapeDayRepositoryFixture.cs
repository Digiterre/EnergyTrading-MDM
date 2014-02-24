namespace EnergyTrading.MDM.Test.Data.EF
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RWEST.Nexus.MDM;

    [TestClass]
    public class ShapeDayRepositoryFixture : DbSetRepositoryFixture<ShapeDay>
    {
        protected override ShapeDay Default()
        {
            var entity = ObjectMother.Create<ShapeDay>();

            return entity;
        }
    }
}
