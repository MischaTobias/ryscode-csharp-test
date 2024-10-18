using System;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Implements;

public class FixedRateRebateCalculator : IRebateCalculator
{
    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0 || !product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
        {
            throw new InvalidOperationException("Invalid incentive calculation");
        }
        return product.Price * rebate.Percentage * request.Volume;
    }
}
