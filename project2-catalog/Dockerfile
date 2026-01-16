# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy ServiceDefaults project
COPY aspire-orchestration/JobPortal.ServiceDefaults/JobPortal.ServiceDefaults.csproj aspire-orchestration/JobPortal.ServiceDefaults/

# Copy project files
COPY project2-catalog/src/JobPortal.Catalog.Domain/JobPortal.Catalog.Domain.csproj project2-catalog/src/JobPortal.Catalog.Domain/
COPY project2-catalog/src/JobPortal.Catalog.Data/JobPortal.Catalog.Data.csproj project2-catalog/src/JobPortal.Catalog.Data/
COPY project2-catalog/src/JobPortal.Catalog.Bll/JobPortal.Catalog.Bll.csproj project2-catalog/src/JobPortal.Catalog.Bll/
COPY project2-catalog/src/JobPortal.Catalog.WebApi/JobPortal.Catalog.WebApi.csproj project2-catalog/src/JobPortal.Catalog.WebApi/

# Restore dependencies
RUN dotnet restore project2-catalog/src/JobPortal.Catalog.WebApi/JobPortal.Catalog.WebApi.csproj

# Copy source code
COPY project2-catalog/src/ project2-catalog/src/
# Copy ServiceDefaults source code
COPY aspire-orchestration/JobPortal.ServiceDefaults/ aspire-orchestration/JobPortal.ServiceDefaults/

# Build the project
WORKDIR /src/project2-catalog/src/JobPortal.Catalog.WebApi
RUN dotnet build JobPortal.Catalog.WebApi.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish JobPortal.Catalog.WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobPortal.Catalog.WebApi.dll"]
