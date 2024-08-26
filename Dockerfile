# Base image with .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src

# Copy project files and restore dependencies
COPY ["./DatabasesBenchmark.API/DatabasesBenchmark.API.csproj", "DatabasesBenchmark.API/"]
COPY ["./DatabasesBenchmark.Services/DatabasesBenchmark.Services.csproj", "DatabasesBenchmark.Services/"]
COPY ["./DatabasesBenchmark.Domain/DatabasesBenchmark.Domain.csproj", "DatabasesBenchmark.Domain/"]
COPY ["./DatabasesBenchmark.Infrastructure/DatabasesBenchmark.Infrastructure.csproj", "DatabasesBenchmark.Infrastructure/"]

RUN dotnet restore "./DatabasesBenchmark.API/DatabasesBenchmark.API.csproj"

# Copy the remaining source code and build
COPY . .
WORKDIR "/src/DatabasesBenchmark.API"
RUN dotnet build "./DatabasesBenchmark.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./DatabasesBenchmark.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage image with runtime and application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "DatabasesBenchmark.API.dll"]
