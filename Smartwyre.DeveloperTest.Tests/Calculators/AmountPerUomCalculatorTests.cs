using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class AmountPerUomCalculatorTests
{
    private readonly AmountPerUomCalculator _sut = new();

    private static Product ProductWith(SupportedIncentiveType supported)
        => new Product { SupportedIncentives = supported };

    private static Rebate RebateWith(decimal amount = 5m)
        => new Rebate { Amount = amount, Incentive = IncentiveType.AmountPerUom };

    [Fact]
    public void CanCalculate_Fails_When_NotSupported()
    {
        var product = ProductWith(SupportedIncentiveType.None);
        var rebate = RebateWith();

        var ok = _sut.CanCalculate(product, rebate, new CalculateRebateRequest { Volume = 1 });
                
        Assert.Equal("This product don't support AmountPerUom.", ok.Reason);
        Assert.False(ok.IsValid);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(5, 0)]
    public void CanCalculate_Fails_When_Invalid(decimal amount, int volume)
    {
        var product = ProductWith(SupportedIncentiveType.AmountPerUom);
        var rebate = RebateWith(amount);

        var ok = _sut.CanCalculate(product, rebate, new CalculateRebateRequest { Volume = volume });

        Assert.Equal("Invalid Parameters.", ok.Reason);
        Assert.False(ok.IsValid);
    }

    [Fact]
    public void Calculate_Ok()
    {
        var product = ProductWith(SupportedIncentiveType.AmountPerUom);
        var rebate = RebateWith(3m);
        var req = new CalculateRebateRequest { Volume = 4 };

        var total = _sut.Calculate(product, rebate, req);

        Assert.Equal(12m, total);
    }
}
