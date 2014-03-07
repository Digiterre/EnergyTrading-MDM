﻿namespace EnergyTrading.MDM.Test.Contracts.Validators
{
    using RWEST.Nexus.MDM.Contracts;

    public partial class ExchangeValidatorFixture
    {
        partial void AddRelatedEntities(RWEST.Nexus.MDM.Contracts.Exchange contract)
        {
            contract.Party = new EntityId() { Identifier = new NexusId() { IsNexusId = true, Identifier = "1" } };
        }
    }
}