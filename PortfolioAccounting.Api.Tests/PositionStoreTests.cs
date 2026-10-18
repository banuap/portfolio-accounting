using PortfolioAccounting.Api;
using Xunit;

namespace PortfolioAccounting.Api.Tests;

public class PositionStoreTests
{
    [Fact]
    public void CalculatesCostBasisMarketValueAndUnrealizedGainLoss()
    {
        var store = new PositionStore();

        var position = store.Add(new CreatePositionRequest(
            "demo-account-001", "acme", 10m, 12.50m, 15m));

        Assert.Equal(125m, position.CostBasis);
        Assert.Equal(150m, position.MarketValue);
        Assert.Equal(25m, position.UnrealizedGainLoss);
        Assert.Equal("ACME", position.SecuritySymbol);
    }

    [Fact]
    public void FiltersPositionsByAccountReference()
    {
        var store = new PositionStore();
        store.Add(new CreatePositionRequest("demo-account-001", "ACME", 1m, 10m, 11m));
        store.Add(new CreatePositionRequest("demo-account-002", "XYZ", 2m, 20m, 19m));

        var positions = store.List("demo-account-001");

        Assert.Single(positions);
        Assert.Equal("ACME", positions.Single().SecuritySymbol);
    }
}
