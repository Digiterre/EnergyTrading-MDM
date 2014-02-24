namespace EnergyTrading.MDM.Contracts.Mappers
{
    using System.Collections.Generic;

    using EnergyTrading.Data;
    using RWEST.Nexus.MDM.Contracts;
    using EnergyTrading.MDM.Data;
    using EnergyTrading.Mapping;
	
    /// <summary>
    /// Maps a <see cref="SourceSystem" /> to a <see cref="BusinessUnit" />
    /// </summary>
    public class BusinessUnitMapper : ContractMapper<BusinessUnit, MDM.BusinessUnit, BusinessUnitDetails, MDM.BusinessUnitDetails, PartyRoleMapping>
    {
        private readonly IRepository repository;

        public BusinessUnitMapper(IRepository repository, IMappingEngine mappingEngine) : base(mappingEngine)
        {
            this.repository = repository;
        }

        public override void Map(RWEST.Nexus.MDM.Contracts.BusinessUnit source, MDM.BusinessUnit destination)
        {
            base.Map(source, destination);
            destination.PartyRoleType = "BusinessUnit";
            destination.Party = this.repository.FindEntityByMapping<MDM.Party, PartyMapping>(source.Party);
        }

        protected override BusinessUnitDetails ContractDetails(BusinessUnit contract)
        {
            return contract.Details;
        }

        protected override EnergyTrading.DateRange ContractDetailsValidity(BusinessUnit contract)
        {
            return this.SystemDataValidity(contract.Nexus);
        }

        protected override IEnumerable<RWEST.Nexus.MDM.Contracts.NexusId> Identifiers(BusinessUnit contract)
        {
            return contract.Identifiers;
        }
    }
}