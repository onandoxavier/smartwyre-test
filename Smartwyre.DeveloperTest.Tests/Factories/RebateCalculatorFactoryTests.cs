using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Factories;

public class RebateCalculatorFactoryTests
{
    [Fact]
    public void TryGet_Returns_Correct_Calculator_Per_Type()
    {
        var calculators = new IRebateCalculator[]
        {
            new FixedCashAmountCalculator(),
            new FixedRateRebateCalculator(),
            new AmountPerUomCalculator()
        };

        var factory = new RebateCalculatorFactory(calculators);

        var fixedCashAmount = factory.Get(IncentiveType.FixedCashAmount);
        Assert.IsType<FixedCashAmountCalculator>(fixedCashAmount);

        var fixedRateRebate = factory.Get(IncentiveType.FixedRateRebate);
        Assert.IsType<FixedRateRebateCalculator>(fixedRateRebate);

        var amountPerUomCalculator = factory.Get(IncentiveType.AmountPerUom);
        Assert.IsType<AmountPerUomCalculator>(amountPerUomCalculator);
    }

    [Fact]
    public void TryGet_Returns_False_When_Unknown_Type()
    {
        var factory = new RebateCalculatorFactory(Array.Empty<IRebateCalculator>());

        var result = factory.Get((IncentiveType)999);

        Assert.Null(result);
    }
}
