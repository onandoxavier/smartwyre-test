using Moq;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services;

public class RebateServiceTests
{    
    private readonly Mock<IRebateCalculatorFactory> _factory = new();

    private RebateService CreateSut()
        => new RebateService(_factory.Object);

    [Fact]
    public async Task Calculate_Fails_When_Rebate_NotFound()
    {
        // arrange        
        var sut = CreateSut();

        // act
        var result = sut.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "TestRebate0",
            ProductIdentifier = "TestProduct1",
            Volume = 1
        });

        // assert
        Assert.False(result.Success);
        Assert.Equal("Rebate invalid.", result.Reason);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Calculate_Fails_When_Product_NotFound()
    {
        // arrange
        
        /*
         In a scenario where I had applied a repository pattern to retrieve 
         the Rebate and Product entities, I could mock it in this way 
         in order to fine-tune the service's unit tests.
        */
                
        //_rebateRepo.Setup(r => r.GetByIdAsync("TestRebate1", It.IsAny<CancellationToken>()))
        //           .ReturnsAsync(new Rebate { Incentive = IncentiveType.FixedRateRebate });

        //_productRepo.Setup(p => p.GetByIdAsync("TestProduct0", It.IsAny<CancellationToken>()))
        //            .ReturnsAsync((Product?)null);

        var sut = CreateSut();

        // act
        var result = sut.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "TestRebate1",
            ProductIdentifier = "TestProduct0",
            Volume = 1
        });

        // assert
        Assert.False(result.Success);
        Assert.Equal("Product invalid.", result.Reason);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Calculate_Fails_When_No_Calculator_Registered()
    {
        // arrange        
        _factory
            .Setup(f => f.Get(It.IsAny<IncentiveType>()))
            .Returns((IRebateCalculator)null); 

        var sut = CreateSut();

        // act & assert
        Assert.Throws<NotImplementedException>(() =>
            sut.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "TestRebate1",
                ProductIdentifier = "TestProduct1",
                Volume = 1
            }));
    }
}
