using System.Collections.Concurrent;

namespace PortfolioAccounting.Api;

public sealed class PositionStore
{
    private readonly ConcurrentDictionary<Guid, Position> _positions = new();

    public IReadOnlyCollection<Position> List(string? accountReference = null)
    {
        IEnumerable<Position> positions = _positions.Values;
        if (!string.IsNullOrWhiteSpace(accountReference))
        {
            positions = positions.Where(position =>
                string.Equals(position.AccountReference, accountReference, StringComparison.Ordinal));
        }

        return positions.OrderBy(position => position.AccountReference)
            .ThenBy(position => position.SecuritySymbol)
            .ToArray();
    }

    public Position Add(CreatePositionRequest request)
    {
        var position = new Position(
            Guid.NewGuid(),
            request.AccountReference.Trim(),
            request.SecuritySymbol.Trim().ToUpperInvariant(),
            request.Quantity,
            request.UnitCost,
            request.MarketPrice);
        _positions[position.Id] = position;
        return position;
    }
}
