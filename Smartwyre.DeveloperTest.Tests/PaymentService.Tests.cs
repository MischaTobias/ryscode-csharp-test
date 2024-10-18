using Moq;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Services.Implements;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private readonly RebateService _rebateService;
    private readonly Mock<IRebateDataStore> _rebateDataStoreMock;
    private readonly Mock<IProductDataStore> _productDataStoreMock;
    private readonly Mock<IRebateCalculator> _rebateCalculatorMock;

    public PaymentServiceTests()
    {
        _rebateDataStoreMock = new Mock<IRebateDataStore>();
        _productDataStoreMock = new Mock<IProductDataStore>();
        _rebateCalculatorMock = new Mock<IRebateCalculator>();

        var calculators = new Dictionary<IncentiveType, IRebateCalculator>
            {
                { IncentiveType.FixedRateRebate, _rebateCalculatorMock.Object }
            };

        _rebateService = new RebateService(_rebateDataStoreMock.Object, _productDataStoreMock.Object, calculators);
    }

    [Fact]
    public void Calculate_ValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "REBATE_001", Incentive = IncentiveType.FixedRateRebate, Percentage = 10m };
        var product = new Product { Identifier = "PRODUCT_001", Price = 200m };
        var request = new CalculateRebateRequest { RebateIdentifier = "REBATE_001", ProductIdentifier = "PRODUCT_001", Volume = 5 };

        _rebateDataStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productDataStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(product);
        _rebateCalculatorMock.Setup(x => x.Calculate(rebate, product, request)).Returns(100m); // Simulate calculation result

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(100m, result.RebateAmount);
        _rebateDataStoreMock.Verify(x => x.StoreCalculationResult(rebate, 100m), Times.Once);
    }

    [Fact]
    public void Calculate_RebateNotFound_ShouldReturnFailure()
    {
        // Arrange
        var request = new CalculateRebateRequest { RebateIdentifier = "INVALID_REBATE", ProductIdentifier = "PRODUCT_001", Volume = 5 };

        _rebateDataStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns((Rebate)null); // Simulate rebate not found

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.RebateAmount);
    }

    [Fact]
    public void Calculate_ProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "REBATE_001", Incentive = IncentiveType.FixedRateRebate, Percentage = 10m };
        var request = new CalculateRebateRequest { RebateIdentifier = "REBATE_001", ProductIdentifier = "INVALID_PRODUCT", Volume = 5 };

        _rebateDataStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productDataStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns((Product)null); // Simulate product not found

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.RebateAmount);
    }

    [Fact]
    public void Calculate_UnsupportedIncentiveType_ShouldReturnFailure()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "REBATE_001", Incentive = IncentiveType.AmountPerUom }; // Unsupported incentive
        var product = new Product { Identifier = "PRODUCT_001", Price = 200m };
        var request = new CalculateRebateRequest { RebateIdentifier = "REBATE_001", ProductIdentifier = "PRODUCT_001", Volume = 5 };

        _rebateDataStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productDataStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(product);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.RebateAmount);
    }

    [Fact]
    public void Calculate_CalculatorThrowsException_ShouldReturnFailureWithErrorMessage()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "REBATE_001", Incentive = IncentiveType.FixedRateRebate, Percentage = 10m };
        var product = new Product { Identifier = "PRODUCT_001", Price = 200m };
        var request = new CalculateRebateRequest { RebateIdentifier = "REBATE_001", ProductIdentifier = "PRODUCT_001", Volume = 5 };

        _rebateDataStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productDataStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(product);
        _rebateCalculatorMock.Setup(x => x.Calculate(rebate, product, request)).Throws(new InvalidOperationException("Invalid calculation"));

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid calculation", result.ErrorMessage);
    }
}
