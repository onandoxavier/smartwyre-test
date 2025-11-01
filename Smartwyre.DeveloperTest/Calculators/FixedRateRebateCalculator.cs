using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedRateRebateCalculator : IRebateCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

    public ValidationResult CanCalculate(Product product, Rebate rebate, CalculateRebateRequest req)
    {
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
            return ValidationResult.Fail("This product don't support FixedRateRebate.");

        if (rebate.Percentage <= 0 || product.Price <= 0 || req.Volume <= 0)
            return ValidationResult.Fail("Invalid Parameters.");

        return ValidationResult.Ok();
    }

    public decimal Calculate(Product product, Rebate rebate, CalculateRebateRequest req)
        => product.Price * rebate.Percentage * req.Volume;
}
