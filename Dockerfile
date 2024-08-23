# Imagen base de ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 8080

# Imagen de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos los archivos .csproj
COPY ["DatabasesBenchmark.API/DatabasesBenchmark.API.csproj", "DatabasesBenchmark.API/"]
COPY ["DatabasesBenchmark.Services/DatabasesBenchmark.Services.csproj", "DatabasesBenchmark.Services/"]
COPY ["DatabasesBenchmark.Domain/DatabasesBenchmark.Domain.csproj", "DatabasesBenchmark.Domain/"]
COPY ["DatabasesBenchmark.Infrastructure/DatabasesBenchmark.Infrastructure.csproj", "DatabasesBenchmark.Infrastructure/"]

# Ejecuta dotnet restore
RUN dotnet restore "DatabasesBenchmark.API/DatabasesBenchmark.API.csproj" -v diag --disable-parallel

# Copiamos el resto del código fuente y construimos la aplicación
COPY . .
WORKDIR "/src/DatabasesBenchmark.API"
RUN ls -la /src/DatabasesBenchmark.API
RUN dotnet build "DatabasesBenchmark.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicamos la aplicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DatabasesBenchmark.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DatabasesBenchmark.API.dll"]
