# syntax=docker/dockerfile:1

# ── Build stage ───────────────────────────────────────────────────────────
# Target framework for API.csproj (and every Modules/Shared project) is net10.0.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Full repo copied in (API depends on sibling Modules/ and Shared/ projects).
COPY . .

RUN dotnet restore "API/API.csproj"

RUN dotnet publish "API/API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

USER $APP_UID

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "API.dll"]
