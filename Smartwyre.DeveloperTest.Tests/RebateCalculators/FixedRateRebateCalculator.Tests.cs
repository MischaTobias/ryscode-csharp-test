using System;
using Xunit;
using Smartwyre.DeveloperTest.Services.Implements;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests.RebateCalculators;
public class FixedRateRebateCalculatorTests
{
    private readonly FixedRateRebateCalculator _calculator;

    public FixedRateRebateCalculatorTests()
    {
        _calculator = new FixedRateRebateCalculator();
    }

    [Fact]
    public void Calculate_ValidInput_ShouldReturnExpectedResult()
    {
        // Arrange
        var rebate = new Rebate { Percentage = 0.1m }; // 10%
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 200m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 5 // Valid volume
        };

        // Act
        var result = _calculator.Calculate(rebate, product, request);

        // Assert
        Assert.Equal(100m, result); // 200 * 0.1 * 5 = 100
    }

    [Fact]
    public void Calculate_ZeroPercentageInRebate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Percentage = 0m };
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 200m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 5 // Valid volume
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }

    [Fact]
    public void Calculate_ZeroPriceInProduct_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Percentage = 10m }; // 10%
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 0m, // Zero price
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 5 // Valid volume
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }

    [Fact]
    public void Calculate_ZeroVolumeInRequest_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Percentage = 10m }; // 10%
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 200m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 0 // Zero volume
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }

    [Fact]
    public void Calculate_UnsupportedIncentiveType_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rebate = new Rebate { Percentage = 10m }; // 10%
        var product = new Product
        {
            Identifier = "PRODUCT_001",
            Price = 200m,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom // Not supported
        };
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE_001",
            ProductIdentifier = "PRODUCT_001",
            Volume = 5 // Valid volume
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(rebate, product, request));
    }
}