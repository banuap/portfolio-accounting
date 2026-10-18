FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY PortfolioAccounting.Api/PortfolioAccounting.Api.csproj PortfolioAccounting.Api/
COPY PortfolioAccounting.Api.Tests/PortfolioAccounting.Api.Tests.csproj PortfolioAccounting.Api.Tests/
RUN dotnet restore PortfolioAccounting.Api.Tests/PortfolioAccounting.Api.Tests.csproj
COPY PortfolioAccounting.Api/ PortfolioAccounting.Api/
COPY PortfolioAccounting.Api.Tests/ PortfolioAccounting.Api.Tests/
RUN dotnet test PortfolioAccounting.Api.Tests/PortfolioAccounting.Api.Tests.csproj --no-restore
RUN dotnet publish PortfolioAccounting.Api/PortfolioAccounting.Api.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "PortfolioAccounting.Api.dll"]
