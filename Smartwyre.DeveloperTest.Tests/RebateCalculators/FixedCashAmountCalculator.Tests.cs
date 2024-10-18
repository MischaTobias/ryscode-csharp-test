using System;
using Xunit;
using Smartwyre.DeveloperTest.Services.Implements;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests.RebateCalculators;
public class FixedCashAmountCalculatorTests
{
    private readonly FixedCashAmountCalculator _calculator;

    public FixedCashAmountCalculatorTests()
    {
        _calculator = new FixedCashAmountCalculator();
    }

    [Fact]
    public void Calculate_ValidInput_ShouldReturnExpectedResult()
    {
        // Arrange
        var rebate = new Rebate { Amount = 100m };
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 200m,
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 5 // Volume is not used for FixedCashAmount calculation
        };

        // Act
        var result = _calculator.Calculate(rebate, product, request);

        // Assert
        Assert.Equal(100m, result);
    }

    [Fact]
    public void Calculate_ZeroAmountInRebate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Amount = 0m };
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 200m,
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 5 // Volume is not used for FixedCashAmount calculation
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }

    [Fact]
    public void Calculate_UnsupportedIncentiveType_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Amount = 100m };
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 200m,
            SupportedIncentives = SupportedIncentiveType.NoSupport // Not supported
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 5 // Volume is not used for FixedCashAmount calculation
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }
}