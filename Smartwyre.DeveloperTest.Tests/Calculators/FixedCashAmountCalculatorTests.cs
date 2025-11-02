using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class FixedCashAmountCalculatorTests
{
    private readonly FixedCashAmountCalculator _sut = new();

    private static Product ProductWith(SupportedIncentiveType supported)
        => new Product { SupportedIncentives = supported };

    private static Rebate RebateWith(decimal amount = 50m)
        => new Rebate { Amount = amount, Incentive = IncentiveType.FixedCashAmount };

    [Fact]
    public void CanCalculate_Fails_When_NotSupported()
    {
        var product = ProductWith(SupportedIncentiveType.None);
        var rebate = RebateWith();

        var ok = _sut.CanCalculate(product, rebate, new CalculateRebateRequest());

        Assert.Equal("This product don't support FixedCashAmount.", ok.Reason);
        Assert.False(ok.IsValid);
    }

    [Fact]
    public void CanCalculate_Fails_When_Amount_Is_Zero_Or_Negative()
    {
        var product = ProductWith(SupportedIncentiveType.FixedCashAmount);
        var rebate = RebateWith(0m);

        var ok = _sut.CanCalculate(product, rebate, new CalculateRebateRequest());

        Assert.Equal("Rebate amount invalid.", ok.Reason);
        Assert.False(ok.IsValid);
    }

    [Fact]
    public void Calculate_Returns_Fixed_Amount()
    {
        var product = ProductWith(SupportedIncentiveType.FixedCashAmount);
        var rebate = RebateWith(80m);

        var amount = _sut.Calculate(product, rebate, new CalculateRebateRequest());
        
        Assert.Equal(80m, amount);
    }
}
