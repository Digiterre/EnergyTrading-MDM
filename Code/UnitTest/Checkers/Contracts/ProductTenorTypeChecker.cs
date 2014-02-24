﻿namespace EnergyTrading.MDM.Test.Checkers.Contract
{
    using EnergyTrading.Test;

    public class ProductTenorTypeChecker : Checker<RWEST.Nexus.MDM.Contracts.ProductTenorType>
    {
        public ProductTenorTypeChecker()
        {
            Compare(x => x.Identifiers);
            Compare(x => x.Details);
            Compare(x => x.Nexus); 
            Compare(x => x.Audit);
            Compare(x => x.Links);		
        }
    }
}
