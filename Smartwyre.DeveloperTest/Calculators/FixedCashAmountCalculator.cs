using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedCashAmountCalculator : IRebateCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;

    public ValidationResult CanCalculate(Product product, Rebate rebate, CalculateRebateRequest req)
    {
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
            return ValidationResult.Fail("This product don't support FixedCashAmount.");

        if (rebate.Amount <= 0)
            return ValidationResult.Fail("Rebate amount invalid.");

        return ValidationResult.Ok();
    }

    public decimal Calculate(Product product, Rebate rebate, CalculateRebateRequest req)
        => rebate.Amount;
}
