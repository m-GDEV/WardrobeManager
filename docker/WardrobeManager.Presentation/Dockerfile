# Build phase
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY  ./WardrobeManager.Shared /src/WardrobeManager.Shared/
COPY  ./WardrobeManager.Presentation /src/WardrobeManager.Presentation/

# Publish phase
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WardrobeManager.Presentation/WardrobeManager.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY --from=publish /app/publish/wwwroot .

# Setup docker entrypoint script (used to dynamically update appsettings.Production.json based on env variable)
RUN apk add jq 
COPY ["./docker/WardrobeManager.Presentation/update-appsettings.sh", "/docker-entrypoint.d/40-script.sh"]


COPY ["./docker/WardrobeManager.Presentation/nginx.conf", "/etc/nginx/nginx.conf"]

EXPOSE 80