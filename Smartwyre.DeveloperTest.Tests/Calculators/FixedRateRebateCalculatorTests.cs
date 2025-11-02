using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class FixedRateRebateCalculatorTests
{
    private readonly FixedRateRebateCalculator _sut = new();

    private static Product ProductWith(SupportedIncentiveType supported, decimal price = 100m)
        => new Product { SupportedIncentives = supported, Price = price };

    private static Rebate RebateWith(decimal percentage = 0.1m)
        => new Rebate { Percentage = percentage, Incentive = IncentiveType.FixedRateRebate };

    [Fact]
    public void CanCalculate_Fails_When_ProductDoesNotSupport()
    {
        var product = ProductWith(supported: SupportedIncentiveType.None);
        var rebate = RebateWith();
        var req = new CalculateRebateRequest { Volume = 1 };

        var ok = _sut.CanCalculate(product, rebate, req);

        Assert.Equal("This product don't support FixedRateRebate.", ok.Reason);
        Assert.False(ok.IsValid);
    }

    [Theory]
    [InlineData(0, 100, 1)]
    [InlineData(0.1, 0, 1)]
    [InlineData(0.1, 100, 0)]
    public void CanCalculate_Fails_When_InvalidParams(decimal percentage, decimal price, int volume)
    {
        var product = ProductWith(SupportedIncentiveType.FixedRateRebate, price);
        var rebate = RebateWith(percentage);
        var req = new CalculateRebateRequest { Volume = volume };

        var ok = _sut.CanCalculate(product, rebate, req);
        
        Assert.Equal("Invalid Parameters.", ok.Reason);
        Assert.False(ok.IsValid);
    }

    [Fact]
    public void Calculate_Ok_With_ValidData()
    {
        var product = ProductWith(SupportedIncentiveType.FixedRateRebate, price: 200m);
        var rebate = RebateWith(percentage: 0.25m);
        var req = new CalculateRebateRequest { Volume = 2 };

        var amount = _sut.Calculate(product, rebate, req);

        Assert.Equal(100m, amount); // 200 * 0.25 * 2 = 100
    }
}
