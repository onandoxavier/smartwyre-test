using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                // Set all dependecy injections
                services.AddRebates();                 
                services.AddLogging(cfg => cfg.AddConsole());
            })
            .Build();
                
        using var scope = host.Services.CreateScope();
        var sp = scope.ServiceProvider;

        var rebateService = sp.GetRequiredService<IRebateService>();
        
        var req = new CalculateRebateRequest
        {
            RebateIdentifier = "TestRebate1",
            ProductIdentifier = "TestProduct1",
            Volume = 0
        };

        var result = rebateService.Calculate(req);

        Console.WriteLine(result.Success
            ? $"OK"
            : $"ERRO - {result.Reason}");
    }
}
