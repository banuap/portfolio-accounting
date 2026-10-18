using PortfolioAccounting.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PositionStore>();
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/v1/positions", (string? accountReference, PositionStore store) =>
    Results.Ok(store.List(accountReference)));

app.MapPost("/api/v1/positions", CreatePosition);

app.Run();

static IResult CreatePosition(CreatePositionRequest request, PositionStore store)
{
    var errors = Validate(request);
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    var position = store.Add(request);
    return Results.Created($"/api/v1/positions/{position.Id}", position);
}

static Dictionary<string, string[]> Validate(CreatePositionRequest request)
{
    var errors = new Dictionary<string, List<string>>(StringComparer.Ordinal);
    void AddError(string field, string message)
    {
        if (!errors.TryGetValue(field, out var messages))
        {
            messages = [];
            errors[field] = messages;
        }

        messages.Add(message);
    }

    if (string.IsNullOrWhiteSpace(request.AccountReference))
    {
        AddError(nameof(request.AccountReference), "Account reference is required.");
    }

    if (string.IsNullOrWhiteSpace(request.SecuritySymbol)
        || request.SecuritySymbol.Length > 12
        || request.SecuritySymbol.Any(character => !char.IsAsciiLetterOrDigit(character) && character != '.'))
    {
        AddError(nameof(request.SecuritySymbol), "Security symbol must contain up to 12 letters, digits, or periods.");
    }

    if (request.Quantity <= 0)
    {
        AddError(nameof(request.Quantity), "Quantity must be positive.");
    }

    if (request.UnitCost < 0 || request.MarketPrice < 0)
    {
        if (request.UnitCost < 0)
        {
            AddError(nameof(request.UnitCost), "Unit cost cannot be negative.");
        }

        if (request.MarketPrice < 0)
        {
            AddError(nameof(request.MarketPrice), "Market price cannot be negative.");
        }
    }

    return errors.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray(), StringComparer.Ordinal);
}

public partial class Program
{
}
