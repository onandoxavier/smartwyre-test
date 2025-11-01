using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class AmountPerUomCalculator : IRebateCalculator
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;

    public ValidationResult CanCalculate(Product product, Rebate rebate, CalculateRebateRequest req)
    {
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
            return ValidationResult.Fail("This product don't support AmountPerUom.");

        if (rebate.Amount <= 0 || req.Volume <= 0)
            return ValidationResult.Fail("Invalid Parameters.");

        return ValidationResult.Ok();
    }

    public decimal Calculate(Product product, Rebate rebate, CalculateRebateRequest req)
        => rebate.Amount * req.Volume;
}