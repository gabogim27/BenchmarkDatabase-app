# DatabaseBenchmark API 

## Description

This ASP.NET Core application exposes an API of MySQL and Postgres database benchmark.

## Arquitecture

- **DatabasesBenchmark.API**: Controllers that expose endpoints.
- **DatabasesBenchmark.Services**: Services that contain business logic.
- **DatabasesBenchmark.Infrastructure**: Database context and repository implementations.
- **DatabasesBenchmark.Domain**: Entities and contracts (repository interfaces).

## Prerequisites
- Install MySql database version 8.0.23
- Install Postgres database

## Configuration

1. Configure the ConnectionString in `appsettings.json`.
2. Execute migrations to create database:
   ```bash
   dotnet ef migrations add InitialCreate --context MySqlBenchmarkDbContext
   dotnet ef database update --context MySqlBenchmarkDbContext
   dotnet ef migrations add InitialCreate --context PostgreBenchmarkDbContext
   dotnet ef database update --context PostgreBenchmarkDbContext
   
3. Execute the following command:
   ```bash
   dotnet run

This command will expose the following api url: http://localhost:5192/swagger/index.html

4. Now you can test the endpoints using swagger UI:

5. Execute the following command to stop dotnet process:
   ```bash
    get-process -name "dotnet" | stop-process
