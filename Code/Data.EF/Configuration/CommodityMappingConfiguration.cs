namespace EnergyTrading.MDM.Data.EF.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using RWEST.Nexus.MDM;

    public class CommodityMappingConfiguration : EntityTypeConfiguration<CommodityMapping>
    {
        public CommodityMappingConfiguration()
        {
            this.ToTable("CommodityMapping");

            this.Property(x => x.Id).HasColumnName("CommodityMappingId");
            this.HasRequired(x => x.System).WithMany().Map(x => x.MapKey("SourceSystemId"));
            this.HasRequired(x => x.Commodity).WithMany(y => y.Mappings).Map(x => x.MapKey("CommodityId"));
            this.Property(x => x.Validity.Start).HasColumnName("Start");
            this.Property(x => x.Validity.Finish).HasColumnName("Finish");
            this.Property(x => x.Version).IsRowVersion();
        }
    }
}