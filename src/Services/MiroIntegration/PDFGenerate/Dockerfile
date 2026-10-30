FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY PDFGenerate/PDFGenerate.API.csproj PDFGenerate/
COPY Shared/Shared/Shared.csproj Shared/Shared/
COPY MiroIntegration.Domain/MiroIntegration.Domain.csproj MiroIntegration.Domain/

RUN dotnet restore PDFGenerate/PDFGenerate.API.csproj

COPY . .

RUN dotnet publish PDFGenerate/PDFGenerate.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PDFGenerate.API.dll"]