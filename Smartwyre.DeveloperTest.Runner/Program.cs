using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data.Implements;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Services.Implements;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        // Set up the Dependency Injection container
        var serviceProvider = ConfigureServices();

        // Resolve IRebateService from the service provider
        var rebateService = serviceProvider.GetService<IRebateService>();

        if (rebateService == null)
        {
            Console.WriteLine("Failed to resolve IRebateService.");
            return;
        }

        // Gather user inputs for rebate identifier, product identifier, and volume
        Console.WriteLine("Enter Rebate Identifier:");
        string rebateIdentifier = Console.ReadLine();

        Console.WriteLine("Enter Product Identifier:");
        string productIdentifier = Console.ReadLine();

        Console.WriteLine("Enter Volume:");
        string volumeInput = Console.ReadLine();

        // Validate and parse volume
        if (!decimal.TryParse(volumeInput, out decimal volume))
        {
            Console.WriteLine("Invalid volume entered.");
            return;
        }

        // Create the CalculateRebateRequest with user inputs
        var rebateRequest = new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        };

        // Call the rebate service to calculate the rebate
        CalculateRebateResult result = rebateService.Calculate(rebateRequest);

        // Output the result
        if (result.Success)
        {
            Console.WriteLine("Rebate calculation successful.");
            Console.WriteLine($"Rebate amount: {result.RebateAmount}");
        }
        else
        {
            Console.WriteLine("Rebate calculation failed.");
            Console.WriteLine($"Error message: {result.ErrorMessage}");
        }
    }

    static ServiceProvider ConfigureServices()
    {
        // Create a new ServiceCollection
        var services = new ServiceCollection();

        // Register individual calculators
        services.AddTransient<IRebateCalculator, FixedCashAmountCalculator>();
        services.AddTransient<IRebateCalculator, FixedRateRebateCalculator>();
        services.AddTransient<IRebateCalculator, AmountPerUomCalculator>();

        // Register the dictionary for the strategy pattern
        services.AddSingleton<IDictionary<IncentiveType, IRebateCalculator>>(provider =>
        {
            // Get all registered IRebateCalculator implementations
            var calculators = provider.GetServices<IRebateCalculator>();

            return new Dictionary<IncentiveType, IRebateCalculator>
            {
                { IncentiveType.FixedCashAmount, calculators.OfType<FixedCashAmountCalculator>().First() },
                { IncentiveType.FixedRateRebate, calculators.OfType<FixedRateRebateCalculator>().First() },
                { IncentiveType.AmountPerUom, calculators.OfType<AmountPerUomCalculator>().First() }
            };
        });



        // Register DataServices
        services.AddTransient<IRebateDataStore, RebateDataStore>();
        services.AddTransient<IProductDataStore, ProductDataStore>();

        // Register RebateService
        services.AddSingleton<IRebateService, RebateService>();

        // Build the service provider
        return services.BuildServiceProvider();
    }
}
