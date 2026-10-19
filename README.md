# Portfolio Accounting API

.NET 8 / ASP.NET Core API starter for synthetic portfolio position records.

- `POST /api/v1/positions` validates and records a demo position.
- `GET /api/v1/positions` lists positions; an optional
  `accountReference` query filters the in-memory records.
- Position responses include cost basis, market value, and unrealized gain/loss
  derived from the supplied quantity, unit cost, and market price.
- `GET /health` reports service health.

## Run and test

```sh
dotnet run --project PortfolioAccounting.Api
dotnet test PortfolioAccounting.Api.Tests
```

Positions are held in process memory and reset on restart. Market prices are
caller-supplied synthetic values, not live quotes or official closing prices.
This starter does not implement tax-lot accounting, fee calculation, billing,
authorization, durable storage, or valuation approvals. Use only synthetic
data; do not use the output for investment, tax, or billing decisions.

The GitHub Actions **CodeQL** workflow scans C# on pushes to `main`, pull
requests targeting `main`, manual dispatch, and weekly. Review findings under
**Security → Code scanning** in GitHub.
