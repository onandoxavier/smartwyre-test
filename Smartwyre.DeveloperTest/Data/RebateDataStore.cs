using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore
{
    private readonly List<Rebate> _rebates = new()
    {
        new Rebate
        {
            Identifier = "TestRebate1",
            Incentive = IncentiveType.AmountPerUom,
            Amount = 10m
        },
        new Rebate
        {
            Identifier = "TestRebate2",
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 100m
        },
        new Rebate
        {
            Identifier = "TestRebate3",
            Incentive = IncentiveType.FixedRateRebate,
            Amount = 5m
        }
    };

    public Rebate GetRebate(string rebateIdentifier)
    {
        return _rebates.Find(r => r.Identifier == rebateIdentifier);
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
