namespace EnergyTrading.MDM.Mappers
{
    using EnergyTrading.MDM.Extensions;
    using EnergyTrading.Mapping;

    public class SourceSystemDetailsMapper : Mapper<EnergyTrading.MDM.SourceSystem, EnergyTrading.Mdm.Contracts.SourceSystemDetails>
    {
        public override void Map(EnergyTrading.MDM.SourceSystem source, EnergyTrading.Mdm.Contracts.SourceSystemDetails destination)
        {
            destination.Name = source.Name;
            destination.Parent = source.Parent.CreateNexusEntityId(() => source.Parent.Name);
        }
    }
}		