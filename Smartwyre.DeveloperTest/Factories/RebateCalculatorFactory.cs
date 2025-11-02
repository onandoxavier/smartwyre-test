using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Factories;

public class RebateCalculatorFactory : IRebateCalculatorFactory
{
    private readonly IReadOnlyDictionary<IncentiveType, IRebateCalculator> _map;

    public RebateCalculatorFactory(IEnumerable<IRebateCalculator> calculators)
        => _map = calculators.ToDictionary(c => c.IncentiveType);

    public IRebateCalculator Get(IncentiveType type)
        => _map.TryGetValue(type, out var c) ? c : null;
}
