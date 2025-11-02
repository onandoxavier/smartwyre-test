using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Globalization;
using System.Linq;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        /*
         I added the host layer to work with dependency injection; 
         I believe it's a good development practice worth incorporating into the project, 
         and it's important for testing, as I point out in some scenarios where I use a 
         concrete DataStore instead of a repository pattern. 
        */
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddRebates();
                services.AddLogging(cfg => cfg.AddConsole());
            })
            .Build();

        using var scope = host.Services.CreateScope();
        var sp = scope.ServiceProvider;

        var rebateService = sp.GetRequiredService<IRebateService>();


        /*
         The interaction code via the prompt was generated almost entirely 
         using an AI service, as I believe it is not relevant for the evaluation. 
        */
        var (rebates, products) = rebateService.GetAvailableRebatesAndProducts();

        Console.WriteLine("\n=== AVAILABLE PRODUCTS ===");
        Console.WriteLine("Id | Identifier     | Price      | UoM  | Supported Incentives");
        Console.WriteLine("---+----------------+------------+------+----------------------");
        foreach (var p in products)
        {
            Console.WriteLine(
                $"{p.Id,2} | {p.Identifier,-14} | {p.Price,10:C2} | {p.Uom,-4} | {p.SupportedIncentives}");
        }

        Console.WriteLine("\n=== AVAILABLE REBATES ===");
        Console.WriteLine("Identifier     | Incentive        | Details");
        Console.WriteLine("----------------+------------------+---------------------------");
        foreach (var r in rebates)
        {
            var details = r.Incentive switch
            {
                IncentiveType.FixedCashAmount => $"Amount={r.Amount}",
                IncentiveType.AmountPerUom => $"Amount/UoM={r.Amount}",
                IncentiveType.FixedRateRebate => $"Percentage={r.Percentage:P2}",
                _ => ""
            };
            Console.WriteLine($"{r.Identifier,-15} | {r.Incentive,-16} | {details}");
        }
  
        var productId = ReadString("Enter Product Identifier: ",
            id => products.Any(r => string.Equals(r.Identifier, id, StringComparison.OrdinalIgnoreCase)),
            "Invalid Product Identifier. Choose one of the listed identifiers.");

        var rebateId = ReadString("Enter Rebate Identifier: ",
            id => rebates.Any(r => string.Equals(r.Identifier, id, StringComparison.OrdinalIgnoreCase)),
            "Invalid Rebate Identifier. Choose one of the listed identifiers.");

        var volume = ReadDecimal("Enter Volume (numeric): ",
            v => v >= 0, "Volume must be >= 0.");

        var req = new CalculateRebateRequest
        {
            ProductIdentifier = products.First(p => p.Identifier == productId).Identifier,
            RebateIdentifier = rebates.First(r => string.Equals(r.Identifier, rebateId, StringComparison.OrdinalIgnoreCase)).Identifier,
            Volume = volume
        };

        Console.WriteLine("\nRunning rebate calculation...\n");

        var result = rebateService.Calculate(req);

        if (result.Success)
        {
            Console.WriteLine("✅ Calculation successful");
        }
        else
        {
            Console.WriteLine("❌ Calculation failed");
            Console.WriteLine($"Reason: {result.Reason}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    // -------- helpers ----------
    static string ReadString(string prompt, Func<string, bool> isValid, string errorMsg)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(s) && isValid(s)) return s.Trim();
            Console.WriteLine(errorMsg);
        }
    }

    static decimal ReadDecimal(string prompt, Func<decimal, bool> isValid, string errorMsg)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var v) && isValid(v))
                return v;

            if (decimal.TryParse(s, out v) && isValid(v)) return v;

            Console.WriteLine(errorMsg);
        }
    }
}
