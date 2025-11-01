using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore
{
    private readonly List<Product> _products = new()
    {
        new Product
        {
            Id  = 1,
            Identifier = "TestProduct1",
            Price = 1000m,
            Uom = "Unit",
            SupportedIncentives = SupportedIncentiveType.AmountPerUom | SupportedIncentiveType.FixedCashAmount,            
            
        },
        new Product
        {
            Id  = 2,
            Identifier = "TestProduct2",
            Price = 500m,
            Uom = "Litre",
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate | SupportedIncentiveType.AmountPerUom,
        },
        new Product
        {
            Id  = 3,
            Identifier = "TestProduct3",
            Price = 750m,
            Uom = "Kg",
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.FixedRateRebate,
        },
        new Product
        {
            Id  = 4,
            Identifier = "TestProduct4",
            Price = 1200m,
            Uom = "Unit",
            SupportedIncentives = SupportedIncentiveType.None,
        }
    };

    public Product GetProduct(string productIdentifier)
    {
        return _products.Find(p => p.Identifier == productIdentifier);
    }
}
