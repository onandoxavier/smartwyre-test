using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Types;

public class SupportedIncentiveTypeTests
{
    [Fact]
    public void None_Should_Not_Have_Any_Flag()
    {
        var flags = SupportedIncentiveType.None;

        Assert.False(flags.HasFlag(SupportedIncentiveType.FixedCashAmount));
        Assert.False(flags.HasFlag(SupportedIncentiveType.FixedRateRebate));
        Assert.False(flags.HasFlag(SupportedIncentiveType.AmountPerUom));
    }

    [Fact]
    public void Product_With_None_Should_Fail_Validation_For_All_Incentives()
    {
        var product = new Product { SupportedIncentives = SupportedIncentiveType.None, Price = 100m };

        var fixedRateCalc = new FixedRateRebateCalculator();
        var fixedCashCalc = new FixedCashAmountCalculator();
        var perUomCalc = new AmountPerUomCalculator();

        var fixedRate = fixedRateCalc.CanCalculate(product, new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m }, new CalculateRebateRequest { Volume = 1 });
        var fixedCash = fixedCashCalc.CanCalculate(product, new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 10m }, new CalculateRebateRequest { Volume = 1 });
        var perUom = perUomCalc.CanCalculate(product, new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 5m }, new CalculateRebateRequest { Volume = 1 });

        Assert.False(fixedRate.IsValid);
        Assert.False(fixedCash.IsValid);
        Assert.False(perUom.IsValid);
    }
}
