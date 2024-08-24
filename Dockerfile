#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_NOLOGO=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /src

COPY ["./DatabasesBenchmark.API/DatabasesBenchmark.API.csproj", "DatabasesBenchmark.API/"]
COPY ["./DatabasesBenchmark.Services/DatabasesBenchmark.Services.csproj", "DatabasesBenchmark.Services/"]
COPY ["./DatabasesBenchmark.Domain/DatabasesBenchmark.Domain.csproj", "DatabasesBenchmark.Domain/"]
COPY ["./DatabasesBenchmark.Infrastructure/DatabasesBenchmark.Infrastructure.csproj", "DatabasesBenchmark.Infrastructure/"]

RUN dotnet restore "./DatabasesBenchmark.API/DatabasesBenchmark.API.csproj" --no-cache
COPY . .
WORKDIR "/src/DatabasesBenchmark.API"
RUN dotnet build "./DatabasesBenchmark.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./DatabasesBenchmark.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DatabasesBenchmark.API.dll"]