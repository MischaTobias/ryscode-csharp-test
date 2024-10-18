using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data.Implements;

public class ProductDataStore : IProductDataStore
{
    private readonly Dictionary<string, Product> _products = new()
    {
        // FixedCashAmount scenario products
        { "PRODUCT_FIXEDCASH_001", new Product { Identifier = "PRODUCT_FIXEDCASH_001", Price = 1000m, SupportedIncentives = SupportedIncentiveType.NoSupport } },
        { "PRODUCT_FIXEDCASH_002", new Product { Identifier = "PRODUCT_FIXEDCASH_002", Price = 500m, SupportedIncentives = SupportedIncentiveType.FixedCashAmount } },

        // FixedRateRebate scenario products
        { "PRODUCT_FIXEDRATE_001", new Product { Identifier = "PRODUCT_FIXEDRATE_001", Price = 0, SupportedIncentives = SupportedIncentiveType.FixedRateRebate } },
        { "PRODUCT_FIXEDRATE_002", new Product { Identifier = "PRODUCT_FIXEDRATE_002", Price = 1500m, SupportedIncentives = SupportedIncentiveType.NoSupport } },
        { "PRODUCT_FIXEDRATE_003", new Product { Identifier = "PRODUCT_FIXEDRATE_003", Price = 10000m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate } },

        // AmountPerUom scenario products
        { "PRODUCT_AMOUNTUOM_001", new Product { Identifier = "PRODUCT_AMOUNTUOM_001", Price = 50m, SupportedIncentives = SupportedIncentiveType.NoSupport } },
        { "PRODUCT_AMOUNTUOM_002", new Product { Identifier = "PRODUCT_AMOUNTUOM_002", Price = 75m, SupportedIncentives = SupportedIncentiveType.AmountPerUom } },
    };

    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        if (_products.TryGetValue(productIdentifier, out var product))
        {
            return product;
        }

        throw new ArgumentException($"Product not found for identifier: {productIdentifier}");
    }
}
