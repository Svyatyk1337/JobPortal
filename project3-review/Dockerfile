# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY src/JobPortal.Review.Domain/JobPortal.Review.Domain.csproj src/JobPortal.Review.Domain/
COPY src/JobPortal.Review.Application/JobPortal.Review.Application.csproj src/JobPortal.Review.Application/
COPY src/JobPortal.Review.Infrastructure/JobPortal.Review.Infrastructure.csproj src/JobPortal.Review.Infrastructure/
COPY src/JobPortal.Review.WebApi/JobPortal.Review.WebApi.csproj src/JobPortal.Review.WebApi/

# Restore
RUN dotnet restore src/JobPortal.Review.WebApi/JobPortal.Review.WebApi.csproj

# Copy source
COPY src/ src/

# Build
WORKDIR /src/src/JobPortal.Review.WebApi
RUN dotnet build JobPortal.Review.WebApi.csproj -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish JobPortal.Review.WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobPortal.Review.WebApi.dll"]
