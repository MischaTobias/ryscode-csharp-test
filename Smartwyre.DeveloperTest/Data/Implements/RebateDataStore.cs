using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data.Implements;

public class RebateDataStore : IRebateDataStore
{
    private readonly Dictionary<string, Rebate> _rebates = new()
    {
        // FixedCashAmount scenarios
        { "REBATE_FIXEDCASH_001", new Rebate { Identifier = "REBATE_FIXEDCASH_001", Incentive = IncentiveType.FixedCashAmount, Amount = 0 } },
        { "REBATE_FIXEDCASH_002", new Rebate { Identifier = "REBATE_FIXEDCASH_002", Incentive = IncentiveType.FixedCashAmount, Amount = 100m } },

        // FixedRateRebate scenarios
        { "REBATE_FIXEDRATE_001", new Rebate { Identifier = "REBATE_FIXEDRATE_001", Incentive = IncentiveType.FixedRateRebate, Percentage = 0 } },
        { "REBATE_FIXEDRATE_002", new Rebate { Identifier = "REBATE_FIXEDRATE_002", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.05m } },

        // AmountPerUom scenarios
        { "REBATE_AMOUNTUOM_001", new Rebate { Identifier = "REBATE_AMOUNTUOM_001", Incentive = IncentiveType.AmountPerUom, Amount = 0 } },
        { "REBATE_AMOUNTUOM_002", new Rebate { Identifier = "REBATE_AMOUNTUOM_002", Incentive = IncentiveType.AmountPerUom, Amount = 5m } },
    };

    public Rebate GetRebate(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        if (_rebates.TryGetValue(rebateIdentifier, out var rebate))
        {
            return rebate;
        }
        throw new ArgumentException($"Rebate not found for identifier: {rebateIdentifier}");
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
