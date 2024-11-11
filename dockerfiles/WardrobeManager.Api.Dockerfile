FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

RUN echo "got here"
# This copy stuff will only work when the docker build context is one level higher than where this dockerfile is located. You can do this with the docker-compose.yml
COPY  ./WardrobeManager.Shared /src/WardrobeManager.Shared/
COPY  ./WardrobeManager.Api /src/WardrobeManager.Api/

# idk why they recommend building and then publishing
WORKDIR /src
RUN dotnet build "./WardrobeManager.Api/WardrobeManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WardrobeManager.Api/WardrobeManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WardrobeManager.Api.dll"]

EXPOSE 6000
