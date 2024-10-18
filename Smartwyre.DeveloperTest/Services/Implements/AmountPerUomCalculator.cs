using System;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Implements;

public class AmountPerUomCalculator : IRebateCalculator
{
    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate.Amount == 0 || request.Volume == 0 || !product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
        {
            throw new InvalidOperationException("Invalid incentive calculation");
        }
        return rebate.Amount * request.Volume;
    }
}
