using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateCalculatorFactory _factory;
    public RebateService(IRebateCalculatorFactory factory)
    {
        _factory = factory;
    }
    /*
     To handle the potential increase in new IncentiveTypes, 
     I decided to follow a combination of Factory and strategy pattern. 
     This way, it's possible to keep the logic separate, isolate it, 
     keep the service more concise, and make it easier to add new rebates.
    */
    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult { Success = false };
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();

        /*
         I opted for an early return to halt the operation as soon as possible to save resources.
        */
        Rebate rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
        if (rebate == null)
        {
            result.Reason = "Rebate invalid.";
            return result;
        }
        /*
         I also chose to work with a resultPatter pattern to avoid unnecessary exceptions in the flow.
        */
        Product product = productDataStore.GetProduct(request.ProductIdentifier);
        if (product == null)
        {
            result.Reason = "Product invalid.";
            return result;
        }

        /*
         I've made an exception here because it's strictly crucial that when adding a new incentiveType, 
         its respective strategy must also be developed.
        */
        var calc = _factory.Get(rebate.Incentive);
        if (calc == null)        
            throw new NotImplementedException();

        // NOTE:
        // This enum uses bit flags to combine multiple incentive types efficiently.
        // It works well for a small, fixed set — ideally up to 10–12 types.
        // If more incentive types are expected, consider using a collection-based approach
        // (e.g., a List or separate relation) instead of bit flags.
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

    /*
     I made this method available simply to facilitate interaction with mocked data from the console application.
    */
    public (List<Rebate>, List<Product>) GetAvailableRebatesAndProducts()
    {
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();

        var rebates = rebateDataStore.GetRebates();
        var products = productDataStore.GetProducts();

        return (rebates, products);
    }
}
