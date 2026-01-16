# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy ServiceDefaults project
COPY aspire-orchestration/JobPortal.ServiceDefaults/JobPortal.ServiceDefaults.csproj aspire-orchestration/JobPortal.ServiceDefaults/

# Copy project files
COPY project3-review/src/JobPortal.Review.Domain/JobPortal.Review.Domain.csproj project3-review/src/JobPortal.Review.Domain/
COPY project3-review/src/JobPortal.Review.Application/JobPortal.Review.Application.csproj project3-review/src/JobPortal.Review.Application/
COPY project3-review/src/JobPortal.Review.Infrastructure/JobPortal.Review.Infrastructure.csproj project3-review/src/JobPortal.Review.Infrastructure/
COPY project3-review/src/JobPortal.Review.WebApi/JobPortal.Review.WebApi.csproj project3-review/src/JobPortal.Review.WebApi/

# Restore
RUN dotnet restore project3-review/src/JobPortal.Review.WebApi/JobPortal.Review.WebApi.csproj

# Copy source
COPY project3-review/src/ project3-review/src/
# Copy ServiceDefaults source code
COPY aspire-orchestration/JobPortal.ServiceDefaults/ aspire-orchestration/JobPortal.ServiceDefaults/

# Build
WORKDIR /src/project3-review/src/JobPortal.Review.WebApi
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
