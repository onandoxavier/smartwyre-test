using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services;

namespace Smartwyre.DeveloperTest.Runner;

public static class RebateModule
{
    public static IServiceCollection AddRebates(this IServiceCollection services)
    {
        // Calculators: stateless => Singleton
        services.AddSingleton<IRebateCalculator, FixedCashAmountCalculator>();
        services.AddSingleton<IRebateCalculator, FixedRateRebateCalculator>();
        services.AddSingleton<IRebateCalculator, AmountPerUomCalculator>();

        // Factory that use IEnumerable<IRebateCalculator>
        services.AddSingleton<IRebateCalculatorFactory, RebateCalculatorFactory>();

        // application service
        services.AddScoped<IRebateService, RebateService>();

        return services;
    }
}
