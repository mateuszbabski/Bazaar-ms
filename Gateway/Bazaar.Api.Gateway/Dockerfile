FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gateway/Bazaar.Api.Gateway/Bazaar.Api.Gateway.csproj", "Gateway/Bazaar.Api.Gateway/"]
COPY ["Shared/Bazaar.Shared.Infrastructure/Bazaar.Shared.Infrastructure.csproj", "Shared/Bazaar.Shared.Infrastructure/"]
RUN dotnet restore "./Gateway/Bazaar.Api.Gateway/Bazaar.Api.Gateway.csproj"
COPY . .
WORKDIR "/src/Gateway/Bazaar.Api.Gateway"
RUN dotnet build "./Bazaar.Api.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bazaar.Api.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bazaar.Api.Gateway.dll"]