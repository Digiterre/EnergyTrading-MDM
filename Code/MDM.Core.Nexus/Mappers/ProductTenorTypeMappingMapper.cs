﻿namespace EnergyTrading.MDM.Mappers
{
    using OpenNexus.MDM.Contracts;
    using EnergyTrading.Mapping;

    public class ProductTenorTypeMappingMapper: Mapper<EnergyTrading.MDM.ProductTenorTypeMapping, EnergyTrading.Mdm.Contracts.MdmId>
    {
        private readonly Mapper<IEntityMapping, EnergyTrading.Mdm.Contracts.MdmId> mapper;

        public ProductTenorTypeMappingMapper()
        {
            this.mapper = new EntityMappingMapper();
        }

        public override void Map(EnergyTrading.MDM.ProductTenorTypeMapping source, EnergyTrading.Mdm.Contracts.MdmId destination)
        {
            this.mapper.Map(source, destination);
        }
    }
}