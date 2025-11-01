using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Factories;

public interface IRebateCalculatorFactory
{
    IRebateCalculator Get(IncentiveType type);
}
