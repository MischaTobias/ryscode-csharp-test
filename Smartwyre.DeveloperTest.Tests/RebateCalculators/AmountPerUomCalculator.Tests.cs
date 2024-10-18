using System;
using Xunit;
using Smartwyre.DeveloperTest.Services.Implements;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests.RebateCalculators;
public class AmountPerUomCalculatorTests
{
    private readonly AmountPerUomCalculator _calculator;

    public AmountPerUomCalculatorTests()
    {
        _calculator = new AmountPerUomCalculator();
    }

    [Fact]
    public void Calculate_ValidInput_ShouldReturnExpectedResult()
    {
        // Arrange
        var rebate = new Rebate { Amount = 5m };
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 100m,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 10
        };

        // Act
        var result = _calculator.Calculate(rebate, product, request);

        // Assert
        Assert.Equal(50m, result);
    }

    [Fact]
    public void Calculate_ZeroAmountInRebate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Amount = 0m };
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 100m,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 10
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }

    [Fact]
    public void Calculate_ZeroVolumeInRequest_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Amount = 5m };
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 100m,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 0
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }

    [Fact]
    public void Calculate_UnsupportedIncentiveType_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Amount = 5m };
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 100m,
            SupportedIncentives = SupportedIncentiveType.NoSupport
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 10
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }
}
