# Job Portal

A microservices-based job portal platform built with .NET 8 and .NET Aspire.

## Architecture

The system consists of three microservices with polyglot persistence:

| Service | Database | ORM/Data Access |
|---------|----------|-----------------|
| Application Service | PostgreSQL | ADO.NET + Dapper |
| Catalog Service | MySQL | Entity Framework Core |
| Review Service | MongoDB | MongoDB.Driver |

## Project Structure

```
job-portal/
├── aspire-orchestration/     # .NET Aspire orchestration
│   ├── JobPortal.AppHost/    # Orchestrator (starts all services)
│   ├── JobPortal.ServiceDefaults/  # Shared configuration
│   ├── JobPortal.ApiGateway/ # YARP reverse proxy
│   └── JobPortal.Aggregator/ # BFF aggregation service
├── project1-application/     # Application Service (candidates, applications, interviews)
├── project2-catalog/         # Catalog Service (companies, jobs)
└── project3-review/          # Review Service (company reviews with CQRS)
```

## Technologies

- **.NET 8** - Runtime
- **.NET Aspire** - Orchestration & Service Discovery
- **gRPC** - Service-to-service communication
- **REST API** - External API
- **YARP** - API Gateway reverse proxy
- **Serilog** - Structured logging
- **OpenTelemetry** - Distributed tracing
- **FluentValidation** - Request validation
- **AutoMapper** - Object mapping
- **MediatR** - CQRS pattern (Review Service)

## Running the Application

### Prerequisites

- .NET 8 SDK
- Docker & Docker Compose
- .NET Aspire workload

### Start with Aspire

```bash
cd aspire-orchestration/JobPortal.AppHost
dotnet run
```

### Start with Docker Compose

```bash
cd aspire-orchestration
docker-compose up -d
```

## API Endpoints

- **API Gateway**: `http://localhost:5000`
- **Application Service**: `http://localhost:5001`
- **Catalog Service**: `http://localhost:5002`
- **Review Service**: `http://localhost:5003`
- **Aggregator Service**: `http://localhost:5004`

## License

MIT
