namespace EnergyTrading.MDM.Contracts.Mappers
{
    using System.Collections.Generic;

    using RWEST.Nexus.MDM.Contracts;
    using EnergyTrading.Mapping;
	
    /// <summary>
    /// Maps a <see cref="SourceSystem" /> to a <see cref="ShapeDay" />
    /// </summary>
    public class ShapeDayMapper : ContractMapper<ShapeDay, MDM.ShapeDay, ShapeDayDetails, MDM.ShapeDay, ShapeDayMapping>
    {
        public ShapeDayMapper(IMappingEngine mappingEngine) : base(mappingEngine)
        {
        }

        protected override ShapeDayDetails ContractDetails(ShapeDay contract)
        {
            return contract.Details;
        }

        protected override EnergyTrading.DateRange ContractDetailsValidity(ShapeDay contract)
        {
            return this.SystemDataValidity(contract.Nexus);
        }

        protected override IEnumerable<RWEST.Nexus.MDM.Contracts.NexusId> Identifiers(ShapeDay contract)
        {
            return contract.Identifiers;
        }
    }
}