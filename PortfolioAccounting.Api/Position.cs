namespace PortfolioAccounting.Api;

public sealed record Position(
    Guid Id,
    string AccountReference,
    string SecuritySymbol,
    decimal Quantity,
    decimal UnitCost,
    decimal MarketPrice)
{
    public decimal CostBasis => decimal.Round(Quantity * UnitCost, 2);
    public decimal MarketValue => decimal.Round(Quantity * MarketPrice, 2);
    public decimal UnrealizedGainLoss => MarketValue - CostBasis;
}

public sealed record CreatePositionRequest(
    string AccountReference,
    string SecuritySymbol,
    decimal Quantity,
    decimal UnitCost,
    decimal MarketPrice);
