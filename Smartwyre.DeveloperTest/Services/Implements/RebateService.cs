using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Implements;

public class RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore, IDictionary<IncentiveType, IRebateCalculator> calculators) : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore = rebateDataStore;
    private readonly IProductDataStore _productDataStore = productDataStore;
    private readonly IDictionary<IncentiveType, IRebateCalculator> _calculators = calculators;

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate == null || product == null)
        {
            return new CalculateRebateResult { Success = false };
        }

        if (!_calculators.TryGetValue(rebate.Incentive, out IRebateCalculator calculator))
        {
            return new CalculateRebateResult { Success = false };
        }

        try
        {
            var rebateAmount = calculator.Calculate(rebate, product, request);
            _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
            return new CalculateRebateResult { Success = true, RebateAmount = rebateAmount };
        }
        catch (InvalidOperationException ex)
        {
            return new CalculateRebateResult { Success = false, ErrorMessage = ex.Message };
        }
    }
}
