FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MiroIntegration.Api/MiroIntegration.API.csproj MiroIntegration.Api/
COPY MiroIntegration.Application/MiroIntegration.Application.csproj MiroIntegration.Application/
COPY MiroIntegration.Domain/MiroIntegration.Domain.csproj MiroIntegration.Domain/
COPY MiroIntegration.Infrastructure/MiroIntegration.Infrastructure.csproj MiroIntegration.Infrastructure/
COPY Shared/Shared/Shared.csproj Shared/Shared/

RUN dotnet restore MiroIntegration.Api/MiroIntegration.API.csproj

COPY . .

RUN dotnet publish MiroIntegration.Api/MiroIntegration.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "MiroIntegration.API.dll"]