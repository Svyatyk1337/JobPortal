# ============================================
# Build Stage
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy ServiceDefaults project
COPY aspire-orchestration/JobPortal.ServiceDefaults/JobPortal.ServiceDefaults.csproj ./aspire-orchestration/JobPortal.ServiceDefaults/

# Copy solution and project files
COPY project1-application/JobPortal.Application.sln ./project1-application/
COPY project1-application/src/JobPortal.Application.Domain/JobPortal.Application.Domain.csproj ./project1-application/src/JobPortal.Application.Domain/
COPY project1-application/src/JobPortal.Application.Dal/JobPortal.Application.Dal.csproj ./project1-application/src/JobPortal.Application.Dal/
COPY project1-application/src/JobPortal.Application.Bll/JobPortal.Application.Bll.csproj ./project1-application/src/JobPortal.Application.Bll/
COPY project1-application/src/JobPortal.Application.Api/JobPortal.Application.Api.csproj ./project1-application/src/JobPortal.Application.Api/

# Restore dependencies
WORKDIR /src/project1-application
RUN dotnet restore

# Copy all source code
COPY project1-application/ ./
# Copy ServiceDefaults source code
WORKDIR /src
COPY aspire-orchestration/JobPortal.ServiceDefaults/ ./aspire-orchestration/JobPortal.ServiceDefaults/

# Build the application
WORKDIR /src/project1-application/src/JobPortal.Application.Api
RUN dotnet build -c Release -o /app/build

# ============================================
# Publish Stage
# ============================================
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ============================================
# Runtime Stage
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "JobPortal.Application.Api.dll"]
