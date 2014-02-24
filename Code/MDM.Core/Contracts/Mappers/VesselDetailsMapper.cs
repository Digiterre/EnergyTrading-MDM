namespace EnergyTrading.MDM.Contracts.Mappers
{
    using System;

    using EnergyTrading.Mapping;

    /// <summary>
	///
	/// </summary>
    public class VesselDetailsMapper : Mapper<RWEST.Nexus.MDM.Contracts.VesselDetails, MDM.Vessel>
    {
        public override void Map(RWEST.Nexus.MDM.Contracts.VesselDetails source, MDM.Vessel destination)
        {
            destination.Name = source.Name;
        }
    }
}