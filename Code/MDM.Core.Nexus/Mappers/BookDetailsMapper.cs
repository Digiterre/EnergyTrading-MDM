﻿namespace EnergyTrading.MDM.Mappers
{
    using EnergyTrading.Mapping;

    /// <summary>
    /// Maps a <see cref="Book" /> to a <see cref="RWEST.Nexus.MDM.Contracts.BookDetails" />
    /// </summary>
    public class BookDetailsMapper : Mapper<EnergyTrading.MDM.Book, OpenNexus.MDM.Contracts.BookDetails>
    {
        public override void Map(EnergyTrading.MDM.Book source, OpenNexus.MDM.Contracts.BookDetails destination)
        {
            destination.Name = source.Name;
        }
    }
}