using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateCalculatorFactory _factory;
    public RebateService(IRebateCalculatorFactory factory)
    {
        _factory = factory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult { Success = false };
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();
        
        Rebate rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
        if (rebate == null)
        {
            result.Reason = "Rebate invalid.";
            return result;
        }

        Product product = productDataStore.GetProduct(request.ProductIdentifier);
        if (product == null)
        {
            result.Reason = "Product invalid.";
            return result;
        }

        var calc = _factory.Get(rebate.Incentive);
        if (calc == null)        
            throw new NotImplementedException();

        var canCalculate = calc.CanCalculate(product, rebate, request);
        if (!canCalculate.IsValid)
        {
            result.Reason = canCalculate.Reason;
            return result;
        }
        
        var amount = calc.Calculate(product, rebate, request);

        var storeRebateDataStore = new RebateDataStore();
        storeRebateDataStore.StoreCalculationResult(rebate, amount);
        
        result.Success = true;
        
        return result;
    }
}
