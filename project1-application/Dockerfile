# ============================================
# Build Stage
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY JobPortal.Application.sln ./
COPY src/JobPortal.Application.Domain/JobPortal.Application.Domain.csproj ./src/JobPortal.Application.Domain/
COPY src/JobPortal.Application.Dal/JobPortal.Application.Dal.csproj ./src/JobPortal.Application.Dal/
COPY src/JobPortal.Application.Bll/JobPortal.Application.Bll.csproj ./src/JobPortal.Application.Bll/
COPY src/JobPortal.Application.Api/JobPortal.Application.Api.csproj ./src/JobPortal.Application.Api/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/src/JobPortal.Application.Api
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
