﻿namespace EnergyTrading.MDM
{
    using EnergyTrading.MDM.Data;

    public interface IRangedChild : IRanged
    {
        IEntity Entity { get; set; }

        ulong Version { get; }
    }
}
