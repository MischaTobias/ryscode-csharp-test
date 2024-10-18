using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Interfaces;

public interface IRebateCalculator
{
    decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}
