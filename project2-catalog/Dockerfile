# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY src/JobPortal.Catalog.Domain/JobPortal.Catalog.Domain.csproj src/JobPortal.Catalog.Domain/
COPY src/JobPortal.Catalog.Data/JobPortal.Catalog.Data.csproj src/JobPortal.Catalog.Data/
COPY src/JobPortal.Catalog.Bll/JobPortal.Catalog.Bll.csproj src/JobPortal.Catalog.Bll/
COPY src/JobPortal.Catalog.WebApi/JobPortal.Catalog.WebApi.csproj src/JobPortal.Catalog.WebApi/

# Restore dependencies
RUN dotnet restore src/JobPortal.Catalog.WebApi/JobPortal.Catalog.WebApi.csproj

# Copy source code
COPY src/ src/

# Build the project
WORKDIR /src/src/JobPortal.Catalog.WebApi
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
