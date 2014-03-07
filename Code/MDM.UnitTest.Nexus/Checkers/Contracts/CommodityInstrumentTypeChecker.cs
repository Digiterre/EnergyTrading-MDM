namespace EnergyTrading.MDM.Test.Checkers.Contract
{
    using EnergyTrading.Test;

    public class CommodityInstrumentTypeChecker : Checker<RWEST.Nexus.MDM.Contracts.CommodityInstrumentType>
    {
        public CommodityInstrumentTypeChecker()
        {
            Compare(x => x.Identifiers);
            Compare(x => x.Details);
            Compare(x => x.Nexus); 
            Compare(x => x.Audit);
            Compare(x => x.Links);		
        }
    }
}