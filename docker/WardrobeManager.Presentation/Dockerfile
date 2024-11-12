# Build phase
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

COPY  ./WardrobeManager.Shared /src/WardrobeManager.Shared/
COPY  ./WardrobeManager.Presentation /src/WardrobeManager.Presentation/

WORKDIR /src

# Publish phase
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WardrobeManager.Presentation/WardrobeManager.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:1.27.2 AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN cp -r /app/wwwroot/* /usr/share/nginx/html

EXPOSE 80