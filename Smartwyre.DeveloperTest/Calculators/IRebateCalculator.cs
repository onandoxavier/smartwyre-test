using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public interface IRebateCalculator
{
    IncentiveType IncentiveType { get; }
    ValidationResult CanCalculate(Product product, Rebate rebate, CalculateRebateRequest req);
    decimal Calculate(Product product, Rebate rebate, CalculateRebateRequest req);
}
